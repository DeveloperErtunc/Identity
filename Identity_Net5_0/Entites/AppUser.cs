using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Identity_Net5_0.Entites
{
    public class AppUser:IdentityUser<int>
    {
        [Range(15,int.MaxValue)]
        public int Age { get; set; }
    }
}
