﻿using System;

namespace Funda.Tasks.Core.Models
{
    public class UpdateTaskModel
    {
        public Guid Id { get; set; }
        public Guid GroupId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
