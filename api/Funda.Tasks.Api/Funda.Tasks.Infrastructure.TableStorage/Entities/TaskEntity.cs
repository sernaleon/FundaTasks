using Microsoft.Azure.Cosmos.Table;

namespace Funda.Tasks.Core
{
    public class TaskEntity : TableEntity
    {
        public string Name { get; set; }
        public TaskEntity() { }
        public TaskEntity(string id, string userId, string name)
        {
            RowKey = id;
            PartitionKey = userId;
            Name = name;
        }
    }
}
