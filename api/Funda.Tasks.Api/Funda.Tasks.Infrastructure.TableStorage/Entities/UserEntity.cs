using Microsoft.Azure.Cosmos.Table;

namespace Funda.Tasks.Core
{
    public class UserEntity : TableEntity
    {
        public UserEntity() { }
        public UserEntity(string id, string name)
        {
            RowKey = id;
            PartitionKey = name;
        }
    }
}
