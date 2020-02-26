using System.ComponentModel.DataAnnotations;

namespace Entities.DataTransferObjects
{
    public class UserForRegisterDto
    {
        [Required]
        [MinLength(3)]
        public string FirstName { get; set; }
        [Required]
        [MinLength(3)]
        public string LastName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [Display(Name = "Compare Password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}