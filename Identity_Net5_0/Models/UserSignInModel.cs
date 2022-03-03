using System.ComponentModel.DataAnnotations;

namespace Identity_Net5_0.Models
{
    public class UserSignInModel
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}
