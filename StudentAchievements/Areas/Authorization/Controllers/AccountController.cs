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
        private ApplicationDbContext context;
        private UserManager<User> userManager;
        private SignInManager<User> signInManager;

        public AccountController(ApplicationDbContext _context, UserManager<User> _userManager, SignInManager<User> _signInManager)
        {
            context = _context;
            userManager = _userManager;
            signInManager = _signInManager;
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
                        return Redirect(loginModel?.ReturnUrl ?? "/Admin/Index");
                    }
                }
            }

            ModelState.AddModelError("", "Неверный Email или пароль.");

            return View(loginModel);
        }

        public async Task<RedirectResult> Logout(string returnUrl = "/")
        {
            await signInManager.SignOutAsync();

            return Redirect(returnUrl);
        }
    }
}
