using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StudentAchievements.Areas.Authorization.Models;

namespace StudentAchievements.Controllers
{
    public class HomeController : Controller
    {
        private UserManager<User> userManager;

        public HomeController(UserManager<User> _userManager)
        {
            userManager = _userManager;
        }

        public ViewResult Index() => View();

        public async Task<IActionResult> Login()
        {
            var user = await userManager.GetUserAsync(User);

            if (user != null)
            {
                var roles = await userManager.GetRolesAsync(user);
                var firstRole = roles[0];

                switch (firstRole)
                {
                    case "Admin":
                        return RedirectToAction("Index", "Admin", new {area = "Admin"});
                    case "Employer":
                        return RedirectToAction("Index", "Admin", new {area = "Employer"});
                    case "Teacher":
                        return RedirectToAction("Index", "Admin", new {area = "Teacher"});
                    case "Student":
                        return RedirectToAction("Index", "Admin", new {area = "Student"});
                }
            }

            return RedirectToAction("Login", "Account", new {area = "Authorization"});
        }
    }
}
