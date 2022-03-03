using Identity_Net5_0.Context;
using Identity_Net5_0.Entites;
using Identity_Net5_0.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;
using Identity_Net5_0.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace Identity_Net5_0.Controllers
{
    public class UserController : Controller
    {
        readonly UserManager<AppUser> _userManager;
        readonly IdentityContext _identityContext;
        readonly SignInManager<AppUser> _signInManager;
        readonly RoleManager<IdentityRole<int>> _roleManager;
        public UserController(UserManager<AppUser> userManager,
            IdentityContext identityContext,
            SignInManager<AppUser> signInManager,
            RoleManager<IdentityRole<int>> roleManager)
        {
            _userManager = userManager;
            _identityContext = identityContext;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(UserCreateModel model)
        {
            if (ModelState.IsValid)
            {
                AppUser user = new AppUser
                {
                    Email = model.Email,
                    UserName = model.UserName,
                    Age = model.Age,
                    PhoneNumber = model.Phone
                };

                IdentityResult identityResult = await _userManager.CreateAsync(user, model.Password);
                if (identityResult.Succeeded)
                {
                    identityResult = await _userManager.AddToRoleAsync(user, Roles.User.ToString());
                    if (identityResult.Succeeded)
                        return RedirectToAction(nameof(SignIn));
                }
                identityResult.Errors.ToList().ForEach(x => ModelState.AddModelError("", x.Description));
            }
            return View(model);
        }
        [HttpGet]
        public IActionResult UserUpdate()
        {
            var user = GetUserInfo(User.Identity.Name, null).FirstOrDefault();
            var userRolesId = user.UserRoleInfos.Select(x => x.RoleId).ToList();
            var roles = _roleManager.Roles.Select(x =>
            new UserRoleInfo
            {
                Name = x.Name,
                RoleId = x.Id,
                IsHave = userRolesId.Contains(x.Id)
            }).ToList();
            return View(new UserUpdateModel
            {
                Age = user.Age,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                UserName = user.UserName,
                UserRoleInfos = roles
            });
        }
        [HttpPost]
        public async Task<IActionResult> UserUpdate(UserUpdateModel model)
        {
            if (model.UserRoleInfos.Where(x => x.IsHave).Count() <= 0)
            {
                ModelState.AddModelError("", "You Have to Have a Role.");
                return View(model);
            }
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                user.Age = model.Age;
                user.PhoneNumber = model.PhoneNumber;
                var identityResult = await _userManager.UpdateAsync(user);
                if (identityResult.Succeeded)
                {
                    var userRoles = await _userManager.GetRolesAsync(user);
                    var userIsHave = model.UserRoleInfos.Where(x => x.IsHave).Select(x => x.Name);
                    IEnumerable<string> newRole = userIsHave.Where(x => !userRoles.Contains(x));
                    IEnumerable<string> deleteRole = userRoles.Where(x => !userIsHave.Contains(x));
                    if (newRole.Count() > 0)
                    {
                        await _userManager.AddToRolesAsync(user, newRole);
                    }
                    if (deleteRole.Count() > 0)
                    {
                        await _userManager.RemoveFromRolesAsync(user, deleteRole);
                    }
                    if(deleteRole.Count() > 0 || newRole.Count() > 0)
                    {
                        await _signInManager.RefreshSignInAsync(user);
                    }
                    return RedirectToAction(nameof(GetUserInfo));
                }
                foreach (var item in identityResult.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }
            }
            return View(model);
        }
        [HttpGet]
        public IActionResult SignIn()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SignIn(UserSignInModel model)
        {
            if (ModelState.IsValid)
            {
                var signInResult = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, false);
                if (signInResult.Succeeded)
                {
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError("", "Password or Username Incorrect");
            }
            return View(model);
        }
        [Authorize]
        [HttpGet]
        public IActionResult Index()
        {
            List<string> roles = new List<string>
            {
                Roles.User.ToString()
            };
            var adminRole = Roles.Admin.ToString();
            if (User.IsInRole(adminRole))
            {
                roles.Add(adminRole);
            }

            return View(GetUserInfo(null, roles));
        }
        [Authorize]
        [HttpGet]
        public IActionResult GetUserInfo()
        {
            return View(GetUserInfo(User.Identity.Name, null).FirstOrDefault());
        }
        [Authorize]
        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(Register));
        }
        private List<UserInfo> GetUserInfo(string UserName, List<string> roles)
        {
            var users = (from user in _identityContext.Users
                         select new UserInfo
                         {
                             UserId = user.Id,
                             UserName = user.UserName,
                             Email = user.Email,
                             Age = user.Age,
                             PhoneNumber = user.PhoneNumber,
                             UserRoleInfos = (from userRole in _identityContext.UserRoles
                                              join role in _identityContext.Roles
                                              on userRole.RoleId equals role.Id
                                              where userRole.UserId == user.Id
                                              select new UserRoleInfo
                                              {
                                                  Name = role.Name,
                                                  RoleId = role.Id
                                              }).ToList(),
                         }).Where(x => (x.UserRoleInfos.Any(x => roles.Contains(x.Name)) || roles == null) && (x.UserName == UserName || UserName == null)).ToList();
            users.ForEach(x => x.Roles = string.Join(" , ", x.UserRoleInfos.Select(x => x.Name)));
            return users;
        }
    }
}
