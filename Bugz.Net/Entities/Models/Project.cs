using System;
using System.Collections.Generic;

namespace Entities.Models
{
    public class Project
    {
        public Guid ProjectId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool Billable { get; set; }
        public ICollection<UserProject> Users { get; set; }
        public ICollection<Issue> Issues { get; set; }
        public ICollection<Task> Tasks { get; set; }
    }
}