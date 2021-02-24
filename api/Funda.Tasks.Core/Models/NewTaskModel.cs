using System;

namespace Funda.Tasks.Core.Models
{
    public class NewTaskModel
    {
        public Guid? GroupId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
