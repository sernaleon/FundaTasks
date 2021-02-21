using Microsoft.Azure.Cosmos.Table;

namespace Funda.Tasks.Core
{
    public class UserTaskEntity : TableEntity
    {
        public string TaskId { get; set; }
        public string Description { get; set; }
        public UserTaskEntity() { }
        public UserTaskEntity(string id, string userId, string taskId, string description) 
        {
            RowKey = id;
            PartitionKey = userId;
            TaskId = taskId;
            Description = description;
        }
    }
}
