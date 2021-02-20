using Microsoft.Azure.Cosmos.Table;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Funda.Tasks.Infrastructure.TableStorage
{
    public interface IDbContext<T> where T : TableEntity, new()
    {
        Task<T> GetAsync(string partitionKey, string rowKey, CancellationToken token);
        Task SetAsync(T userTasksEntity, CancellationToken token);
        Task DeleteAsync(T userTasksEntity, CancellationToken token);
        Task<T[]> GetAllAsync(CancellationToken token);
    }

    public class DbContext<T> : IDbContext<T> where T : TableEntity, new()
    {
        private readonly string _tableName;
        private readonly string _connectionString;
        private readonly ILogger<T> _logger;

        public DbContext(AzureSettings settings, ILogger<T> logger, string tableName)
        {
            _connectionString = settings.StorageConectionString;
            _logger = logger;
            _tableName = tableName;
        }

        public async Task<T> GetAsync(string partitionKey, string rowKey, CancellationToken token)
        {
            try
            {
                var table = CloudTableHelper.GetCloudTable(_connectionString, _tableName);
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
                var table = CloudTableHelper.GetCloudTable(_connectionString, _tableName);
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
                var table = CloudTableHelper.GetCloudTable(_connectionString, _tableName);
                var operation = TableOperation.Delete(userTasksEntity);
                await table.ExecuteAsync(operation, token);
            }
            catch (Exception e)
            {
                _logger.LogError($"Error deleting {nameof(T)} with partition {userTasksEntity.PartitionKey} and rowKey {userTasksEntity.RowKey}", e);
                throw;
            }
        }

        public async Task<T[]> GetAllAsync(CancellationToken token)
        {
            try
            {
                var table = CloudTableHelper.GetCloudTable(_connectionString, _tableName);
                var query = table.CreateQuery<T>();
                var tableResult = await table.ExecuteQueryAsync(query, token);
                return tableResult?.ToArray();
            }
            catch (Exception e)
            {
                _logger.LogError($"Error getting all {nameof(T)}", e);
                throw;
            }
        }
    }
}
