using System.ComponentModel.DataAnnotations;

namespace Entities.DataTransferObjects
{
    public class RoleUpdateDto
    {
        [Required]
        public string[] Roles { get; set; }
    }
}