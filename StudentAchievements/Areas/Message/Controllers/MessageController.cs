using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using StudentAchievements.Areas.Message.Models.ViewModels;

namespace StudentAchievements.Areas.Message.Controllers
{
    [Area("Message")]
    [Authorize(Roles = "Employer, Student")]
    public class MessageController : Controller
    {
        public IActionResult Index() => View(new MessageListViewModel());

        public IActionResult Back()
        {
            if(User.IsInRole("Employer"))
            {
                return RedirectToAction("Index", "Employer", new { area = "Employer" });
            }

            return RedirectToAction("Index", "Student", new { area = "Student" });
        }
    }
}