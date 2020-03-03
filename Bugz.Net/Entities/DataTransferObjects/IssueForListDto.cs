using System;

namespace Entities.DataTransferObjects
{
    public class IssueForListDto
    {
        public Guid IssueId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Severity { get; set; }
        public string Classification { get; set; }
        public string Status { get; set; }
        public DateTime Created { get; set; }
        public DateTime DueDate { get; set; }
    }
}