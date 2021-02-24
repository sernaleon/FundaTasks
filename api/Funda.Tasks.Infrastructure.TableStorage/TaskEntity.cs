using Microsoft.Azure.Cosmos.Table;

namespace Funda.Tasks.Infrastructure.TableStorage
{
    public class TaskEntity : TableEntity
    {
        public string GroupId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
