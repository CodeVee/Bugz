using System;

namespace Entities.DataTransferObjects
{
    public class IssueForDetailedDto : IssueForListDto
    {
        public string Reporter { get; set; }
        public string Assignee { get; set; }
        public string Project { get; set; }
    }
}