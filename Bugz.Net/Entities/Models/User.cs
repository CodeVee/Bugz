using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace Entities.Models
{
    public class User : IdentityUser<Guid>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public ICollection<UserProject> Projects { get; set; }
        public ICollection<Issue> ReportedIssue { get; set; }
        public ICollection<Issue> AssignedIssue { get; set; }
        public ICollection<Task> CreatedTask { get; set; }
        public ICollection<Task> AssignedTask { get; set; }
    }
}