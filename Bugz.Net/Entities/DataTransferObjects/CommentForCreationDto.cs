using System.ComponentModel.DataAnnotations;

namespace Entities.DataTransferObjects
{
    public class CommentForCreationDto
    {
        [Required]
        public string Message { get; set; }
    }
}