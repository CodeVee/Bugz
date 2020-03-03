using System;
using System.Collections.Generic;
using Entities.Models;

namespace Entities.DataTransferObjects
{
    public class ProjectForDetailedDto : ProjectForListDto
    {
        public ICollection<UserForListDto> Users { get; set; }
        public ICollection<IssueForListDto> Issues { get; set; }
    }
}