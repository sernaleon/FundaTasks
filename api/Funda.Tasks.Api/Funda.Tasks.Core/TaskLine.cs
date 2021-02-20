using System;

namespace Funda.Tasks.Core
{
    public class TaskLine
    {
        public TaskType TaskType { get; set; }
        public string Description { get; set; }
        public DateTimeOffset Timestamp { get; set; }
    }
}
