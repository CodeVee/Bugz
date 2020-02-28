using System;
using System.ComponentModel.DataAnnotations;

namespace Entities.DataTransferObjects
{
    public abstract class ProjectDto
    {
        [Required(ErrorMessage = "Title is Required")]
        [StringLength(25, MinimumLength = 5, ErrorMessage = "Title must be between 5 and 25 characters")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Description is Required")]
        [StringLength(100, MinimumLength = 10, ErrorMessage = "Description must be between 10 and 100 characters")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Start Date is Required")]
        public DateTime? StartDate { get; set; }
        [Required(ErrorMessage = "End Date is Required")]
        public DateTime? EndDate { get; set; }
        [Required(ErrorMessage = "Billable is Required")]
        public bool? Billable { get; set; }
    }
}