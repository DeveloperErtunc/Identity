using System.ComponentModel.DataAnnotations;

namespace Identity_Net5_0.Models
{
    public class UserCreateModel
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
        [Required]
        [MinLength(11)]
        [MaxLength(11)]
        public string Phone { get; set; }
        [Range(15, int.MaxValue)]
        public int Age { get; set; }
    }
}
