using System;
using System.ComponentModel.DataAnnotations;

namespace Entities.DataTransferObjects
{
    public abstract class TaskDto
    {
        [Required(ErrorMessage = "Title is Required")]
        [StringLength(25, MinimumLength = 5, ErrorMessage = "Title must be between 5 and 25 characters")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Description is Required")]
        [StringLength(100, MinimumLength = 20, ErrorMessage = "Description must be between 20 and 100 characters")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Status is Required")]
        public string Status { get; set; }
        [Required(ErrorMessage = "Priority is Required")]
        public string Priority { get; set; }
        [Required(ErrorMessage = "Percentage is Required")]
        public string Percentage { get; set; }
        [Required(ErrorMessage = "Start Date is Required")]
        public DateTime? StartDate { get; set; }
        [Required(ErrorMessage = "End Date is Required")]
        public DateTime? EndDate { get; set; }
    }
}