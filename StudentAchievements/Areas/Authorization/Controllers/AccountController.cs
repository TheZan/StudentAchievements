using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using StudentAchievements.Areas.Admin.Controllers;
using StudentAchievements.Areas.Authorization.Models;
using StudentAchievements.Areas.Authorization.Models.ViewModels;
using StudentAchievements.Infrastructure;
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
        private IConfiguration configuration;

        public AccountController(IConfiguration _configuration, IdentityDbContext _context, UserManager<IdentityUser> _userManager, SignInManager<IdentityUser> _signInManager, RoleManager<IdentityRole> _roleManager, IUserRepository _userRepository)
        {
            configuration = _configuration;
            context = _context;
            userManager = _userManager;
            signInManager = _signInManager;
            roleManager = _roleManager;
            userRepository = _userRepository;
        }

        [AllowAnonymous]
        public ViewResult Login() => View(new LoginViewModel());

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
                        if (!await userManager.IsEmailConfirmedAsync(user))
                        {
                            ModelState.AddModelError(string.Empty, "Вы не подтвердили свой email");
                            return View(loginModel);
                        }

                        var roles = await userManager.GetRolesAsync(user);
                        var firstRole = roles[0];

                        switch (firstRole)
                        {
                            case "Admin":
                                return RedirectToAction("Index", "Admin", new {area = "Admin"});
                            case "Employer":
                                return RedirectToAction("Index", "Admin", new { area = "Employer" });
                            case "Teacher":
                                return RedirectToAction("Index", "Admin", new { area = "Teacher" });
                            case "Student":
                                return RedirectToAction("Index", "Admin", new { area = "Student" });
                        }
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
                    UserName = registrationModel.Email,
                    Email = registrationModel.Email
                };

                var result = await userRepository.AddUser(user, registrationModel.Password,
                    new Employer() {Name = registrationModel.UserName, Email = registrationModel.Email});

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "Employer");
                    await signInManager.SignInAsync(user, false);

                    var code = await userManager.GenerateEmailConfirmationTokenAsync(user);

                    var callbackUrl = Url.Action(
                        "ConfirmEmail",
                        "Account",
                        new {userId = user.Id, code = code},
                        protocol: HttpContext.Request.Scheme);

                    EmailService emailService = new EmailService(configuration);

                    await emailService.SendEmailAsync(registrationModel.Email, "Подтверждение учетной записи",
                        $"Подтвердите регистрацию, перейдя по ссылке: <a href='{callbackUrl}'>Подтвердить</a>");

                    return RedirectToAction("Confirm");
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

        [AllowAnonymous]
        public ViewResult Confirm() => View();

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return View("Error");
            }
            var result = await userManager.ConfirmEmailAsync(user, code);
            if (result.Succeeded)
                return RedirectToAction("Login");
            else
                return View("Error");
        }



        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();

            return RedirectToAction("Login");
        }

        [HttpGet]
        [AllowAnonymous]
        public ViewResult ForgotPassword() => View();

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByEmailAsync(model.Email);
                if (user == null || !(await userManager.IsEmailConfirmedAsync(user)))
                {
                    return View("ForgotPasswordConfirmation");
                }

                var code = await userManager.GeneratePasswordResetTokenAsync(user);
                var callbackUrl = Url.Action("ResetPassword", "Account", new { email = user.Email, userId = user.Id, code = code }, protocol: HttpContext.Request.Scheme);
                EmailService emailService = new EmailService(configuration);
                await emailService.SendEmailAsync(model.Email, "Сброс пароля",
                    $"Для сброса пароля перейдите по ссылке: <a href='{callbackUrl}'>Сбросить</a>");
                return View("ForgotPasswordConfirmation");
            }

            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPassword(string code = null, string email = null) => code == null ? View("Error") : View();

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return View("ResetPasswordConfirmation");
            }
            var result = await userManager.ResetPasswordAsync(user, model.Code, model.Password);
            if (result.Succeeded)
            {
                return View("ResetPasswordConfirmation");
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return View(model);
        }
    }
}
