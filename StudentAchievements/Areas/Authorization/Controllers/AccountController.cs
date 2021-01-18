using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StudentAchievements.Areas.Authorization.Models;
using StudentAchievements.Areas.Authorization.Models.ViewModels;
using StudentAchievements.Models;

namespace StudentAchievements.Areas.Authorization.Controllers
{
    [Area("Authorization")]
    [Authorize]
    public class AccountController : Controller
    {
        private IdentityDbContext context;
        private UserManager<IdentityUser> userManager;
        private SignInManager<IdentityUser> signInManager;
        private RoleManager<IdentityRole> roleManager;
        private IUserRepository userRepository;

        public AccountController(IdentityDbContext _context, UserManager<IdentityUser> _userManager, SignInManager<IdentityUser> _signInManager, RoleManager<IdentityRole> _roleManager, IUserRepository _userRepository)
        {
            context = _context;
            userManager = _userManager;
            signInManager = _signInManager;
            roleManager = _roleManager;
            userRepository = _userRepository;
        }

        [AllowAnonymous]
        public ViewResult Login(string returnUrl) => View(new LoginViewModel() { ReturnUrl = returnUrl });

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel loginModel)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByNameAsync(loginModel.Email);

                if (user != null)
                {
                    await signInManager.SignOutAsync();

                    if ((await signInManager.PasswordSignInAsync(user, loginModel.Password, false, false)).Succeeded)
                    {
                        return Redirect(loginModel?.ReturnUrl ?? "/admin");
                    }
                }
            }

            ModelState.AddModelError("", "Неверный Email или пароль.");

            return View(loginModel);
        }

        [AllowAnonymous]
        public ViewResult Registration() => View();

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Registration(RegistrationViewModel registrationModel)
        {
            if (ModelState.IsValid)
            {
                var user = new IdentityUser()
                {
                    UserName = registrationModel.UserName,
                    Email = registrationModel.Email
                };

                var result = await userRepository.AddUser(user, registrationModel.Password,
                    new Employer() {Name = registrationModel.UserName, Email = registrationModel.Email});

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "Employer");
                    await signInManager.SignInAsync(user, false);

                    return RedirectToAction();
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }

            return View(registrationModel);
        }

        public async Task<RedirectResult> Logout(string returnUrl = "/")
        {
            await signInManager.SignOutAsync();

            return Redirect(returnUrl);
        }
    }
}
