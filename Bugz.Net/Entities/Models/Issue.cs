using System;
using System.Collections.Generic;
using Entities.Enumerations;

namespace Entities.Models
{
    public class Issue
    {
        public Guid IssueId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Guid ReporterId { get; set; }
        public User Reporter { get; set; }
        public Guid? AssigneeId { get; set; }
        public User Assignee { get; set; }
        public Severity Severity { get; set; }
        public Classification Classification { get; set; }
        public Stage Status { get; set; }
        public Guid ProjectId { get; set; }
        public Project Project { get; set; }
        public DateTime Created = DateTime.Now;
        public DateTime? DueDate { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public ICollection<History> Activities { get; set; }
    }
}