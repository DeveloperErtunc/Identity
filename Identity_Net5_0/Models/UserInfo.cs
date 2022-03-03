using System.Collections.Generic;

namespace Identity_Net5_0.Models
{
    public class UserInfo
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Roles { get; set; }
        public int Age { get; set; }
        public List<UserRoleInfo> UserRoleInfos { get; set; }
    }
    public class UserRoleInfo
    {
        public bool IsHave { get; set; }
        public int RoleId { get; set; }
        public string Name { get; set; }
    }
}
