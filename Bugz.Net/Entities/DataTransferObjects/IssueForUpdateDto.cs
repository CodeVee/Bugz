using System;

namespace Entities.DataTransferObjects
{
    public class IssueForUpdateDto : IssueDto
    {
        public string AssigneeEmail { get; set; }
    }
}