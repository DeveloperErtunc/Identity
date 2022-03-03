using Identity_Net5_0.Entites;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Identity_Net5_0.Context
{
    public class IdentityContext:IdentityDbContext<AppUser, IdentityRole<int>,int>
    {
        public IdentityContext(DbContextOptions<IdentityContext> options):base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<IdentityRole<int>>().HasData(new List<IdentityRole<int>>
            {
                new IdentityRole<int>("Admin"){Id = 1,NormalizedName = "ADMIN"},
                new IdentityRole<int>("User"){Id = 2,NormalizedName = "USER"},
            });
            base.OnModelCreating(builder);  
        }
    }
}
