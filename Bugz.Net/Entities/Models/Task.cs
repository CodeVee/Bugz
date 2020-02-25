using System;
using System.Collections.Generic;
using Entities.Enumerations;

namespace Entities.Models
{
    public class Task
    {
        public Guid TaskId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Status Status { get; set; }
        public Priority Priority { get; set; }
        public Completion Percentage { get; set; }
        public Guid CreatorId { get; set; }
        public User Creator { get; set; }
        public Guid? AssigneeId { get; set; }
        public User Assignee { get; set; }
        public Guid ProjectId { get; set; }
        public Project Project { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public ICollection<History> Activities { get; set; }
    }
}