using System;

namespace Entities.DataTransferObjects
{
    public class ProjectForDetailedDto
    {
        public Guid ProjectId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool Billable { get; set; }
    }
}