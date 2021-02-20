using Funda.Tasks.Core;
using Microsoft.Azure.Cosmos.Table;

namespace Funda.Tasks.Infrastructure.TableStorage
{
    public class UserTasksEntity : TableEntity
    {
        public UserTasks UserTasks { get; set; }
    }
}
