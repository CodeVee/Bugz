using System;
using System.ComponentModel.DataAnnotations;

namespace Entities.DataTransferObjects
{
    public class ProjectForCreationDto
    {
        [Required]
        [MaxLength(25)]
        [MinLength(5)]
        public string Title { get; set; }
        [Required]
        [MaxLength(100)]
        [MinLength(10)]
        public string Description { get; set; }
        [Required]
        public DateTime? StartDate { get; set; }
        [Required]
        public DateTime? EndDate { get; set; }
        [Required]
        public bool? Billable { get; set; }
    }
}