using Microsoft.Azure.Cosmos.Table;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos.Table.Queryable;
using System.Linq.Expressions;
using System.Collections.Generic;

namespace Funda.Tasks.Infrastructure.TableStorage
{
    public interface IDbContext<T> where T : TableEntity, new()
    {
        Task<T> GetAsync(string partitionKey, string rowKey, CancellationToken token);
        Task SetAsync(T userTasksEntity, CancellationToken token);
        Task DeleteAsync(T userTasksEntity, CancellationToken token);
        Task<List<T>> SelectAsync(CancellationToken token);
        Task<List<T>> SelectWhereAsync(Expression<Func<T, bool>> wherePredicate, CancellationToken token);
    }

    public class DbContext<T> : IDbContext<T> where T : TableEntity, new()
    {
        private readonly DbContextSettings _settings;
        private readonly ILogger<T> _logger;

        public DbContext(DbContextSettings settings, ILogger<T> logger)
        {
            _settings = settings;
            _logger = logger;
        }

        public async Task<T> GetAsync(string partitionKey, string rowKey, CancellationToken token)
        {
            try
            {
                var table = GetCloudTable(_settings.StorageConectionString, _settings.TableName);
                var operation = TableOperation.Retrieve<T>(partitionKey, rowKey);
                var tableResult = await table.ExecuteAsync(operation, token);
                return tableResult?.Result as T;
            }
            catch (Exception e)
            {
                _logger.LogError($"Error getting {nameof(T)} with partition {partitionKey} and rowKey {rowKey}", e);
                throw;
            }
        }

        public async Task SetAsync(T userTasksEntity, CancellationToken token)
        {
            try
            {
                userTasksEntity.ETag = "*";
                var table = GetCloudTable(_settings.StorageConectionString, _settings.TableName);
                var operation = TableOperation.InsertOrMerge(userTasksEntity);
                await table.ExecuteAsync(operation, token);
            }
            catch (Exception e)
            {
                _logger.LogError($"Error setting {nameof(T)} with partition {userTasksEntity.PartitionKey} and rowKey {userTasksEntity.RowKey}", e);
                throw;
            }
        }

        public async Task DeleteAsync(T userTasksEntity, CancellationToken token)
        {
            try
            {
                userTasksEntity.ETag = "*";
                var table = GetCloudTable(_settings.StorageConectionString, _settings.TableName);
                var operation = TableOperation.Delete(userTasksEntity);
                await table.ExecuteAsync(operation, token);
            }
            catch (Exception e)
            {
                _logger.LogError($"Error deleting {nameof(T)} with partition {userTasksEntity.PartitionKey} and rowKey {userTasksEntity.RowKey}", e);
                throw;
            }
        }

        public async Task<List<T>> SelectAsync(CancellationToken token)
        {
            try
            {
                var table = GetCloudTable(_settings.StorageConectionString, _settings.TableName);
                var query = table.CreateQuery<T>();
                var tableResult = await ExecuteQueryAsync(table, query, token);
                return tableResult;
            }
            catch (Exception e)
            {
                _logger.LogError($"Error getting all {nameof(T)}", e);
                throw;
            }
        }

        public async Task<List<T>> SelectWhereAsync(Expression<Func<T, bool>> wherePredicate, CancellationToken token)
        {
            try
            {
                var table = GetCloudTable(_settings.StorageConectionString, _settings.TableName);
                var query = table.CreateQuery<T>().Where(wherePredicate).AsTableQuery();
                var tableResult = await ExecuteQueryAsync(table, query, token);
                return tableResult;
            }
            catch (Exception e)
            {
                _logger.LogError($"Error getting all {nameof(T)}", e);
                throw;
            }
        }

        private static CloudTable GetCloudTable(string connectionString, string tableName)
        {
            var storageAccount = CloudStorageAccount.Parse(connectionString);
            var tableClient = storageAccount.CreateCloudTableClient(new TableClientConfiguration());
            return tableClient.GetTableReference(tableName);
        }

        private static async Task<List<T>> ExecuteQueryAsync(CloudTable table, TableQuery<T> query, CancellationToken ct = default, Action<IList<T>> onProgress = null)
        {
            var items = new List<T>();
            TableContinuationToken token = null;

            do
            {
                var seg = await table.ExecuteQuerySegmentedAsync(query, token);
                token = seg.ContinuationToken;
                items.AddRange(seg);
                onProgress?.Invoke(items);

            } while (token != null && !ct.IsCancellationRequested);

            return items;
        }
    }
}
