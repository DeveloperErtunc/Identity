using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Identity_Net5_0.Models
{
    public class UserUpdateModel
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [MinLength(11)]
        [MaxLength(11)]
        public string PhoneNumber { get; set; }
        [Range(15, int.MaxValue)]
        public int Age { get; set; }
        public List<UserRoleInfo> UserRoleInfos { get; set; }
    }
}
