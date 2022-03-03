using Identity_Net5_0.Context;
using Identity_Net5_0.Entites;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Identity_Net5_0
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSession();
            services.AddIdentity<AppUser,IdentityRole<int>>(
                // set identity lowercase or uppercase or anything.
                //opt =>
                //{
                //    opt.Password.RequireDigit = false;
                //    opt.Password.RequireLowercase = false;
                //}
                ).AddEntityFrameworkStores<IdentityContext>();
            services.AddDbContext<IdentityContext>(opt =>
            {
                opt.UseSqlServer("server=(localdb)\\mssqllocaldb; database=Identitydb; integrated security=true");
            });
            services.AddControllersWithViews();
            services.AddRazorPages().AddRazorRuntimeCompilation();
            services.ConfigureApplicationCookie(opt =>
            {
                opt.LoginPath = new PathString("/User/SignIn");
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseSession();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
             name: "default",
             pattern: "{controller=User}/{action=SignIn}/{id?}");
            });
        }
    }
}
