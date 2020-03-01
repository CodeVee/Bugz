using System;
using System.ComponentModel.DataAnnotations;
using Entities.Enumerations;

namespace Entities.DataTransferObjects
{
    public abstract class IssueDto
    {
        [Required(ErrorMessage = "Title is Required")]
        [StringLength(25, MinimumLength = 5, ErrorMessage = "Title must be between 5 and 25 characters")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Description is Required")]
        [StringLength(100, MinimumLength = 20, ErrorMessage = "Description must be between 20 and 100 characters")]
        public string Description { get; set; }
        public Guid? AssigneeId { get; set; }
        [Required(ErrorMessage = "Severity is Required")]
        public string Severity { get; set; }
        [Required(ErrorMessage = "Classification is Required")]
        public string Classification { get; set; }
        [Required(ErrorMessage = "Status is Required")]
        public string Status { get; set; }
        public DateTime? DueDate { get; set; }
    
    }
}