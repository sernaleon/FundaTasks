using System;

namespace Funda.Tasks.Core
{
    public class TaskLineItem
    {
        public Guid Id;
        public TaskType Task { get; set; }
        public string Description { get; set; }
        public DateTimeOffset Timestamp { get; set; }
    }
}
