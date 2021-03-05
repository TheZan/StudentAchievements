using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using StudentAchievements.Areas.Message.Models.ViewModels;
using StudentAchievements.Models;
using StudentAchievements.Areas.Authorization.Models;

namespace StudentAchievements.Areas.Message.Controllers
{
    [Area("Message")]
    [Authorize(Roles = "Employer, Student")]
    public class MessageController : Controller
    {
        private StudentAchievementsDbContext context;
        private IUserRepository userRepository;

        public MessageController(StudentAchievementsDbContext _context, IUserRepository _userRepository)
        {
            context = _context;
            userRepository = _userRepository;
        }

        public async Task<IActionResult> Index()
        {
            var currentUser = await userRepository.Users.FirstOrDefaultAsync(u => u.Email == User.Identity.Name);

            var viewModel = new List<MessageListViewModel>();

            var chats = context.Chats.Include(m => m.Messages)
                                    .Include(o => o.OneUser)
                                    .Include(t => t.TwoUser)
                                    .Where(p => p.OneUser == currentUser || p.TwoUser == currentUser)
                                    .ToList();

            foreach (var chat in chats)
            {
                viewModel.Add(new MessageListViewModel()
                {
                    Me = currentUser,
                    Companion = chat.OneUser != currentUser ? chat.OneUser : chat.TwoUser,
                    Chat = chat,
                    UnreadMessageCount = chat.Messages.Where(p => p.IsViewed == false && p.Sender != currentUser.Name).Count()
                });
            }

            return View(viewModel);
        }

        public IActionResult Back()
        {
            if (User.IsInRole("Employer"))
            {
                return RedirectToAction("Index", "Employer", new { area = "Employer" });
            }

            return RedirectToAction("Index", "Student", new { area = "Student" });
        }

        [HttpGet]
        public async Task<IActionResult> GetMessages(string id)
        {
            var currentUser = await userRepository.Users.FirstOrDefaultAsync(u => u.Email == User.Identity.Name);

            var companion = await userRepository.Users.FirstOrDefaultAsync(u => u.Id == id);

            var chat = await context.Chats.Include(o => o.OneUser)
                                        .Include(t => t.TwoUser)
                                        .Include(m => m.Messages)
                                        .FirstOrDefaultAsync(p => (p.OneUser == currentUser || p.OneUser == companion) && (p.TwoUser == currentUser || p.TwoUser == companion));

            var model = new MessageListViewModel()
            {
                Me = currentUser,
                Companion = companion,
                Chat = chat
            };

            return PartialView("Messages", model);
        }

        
    }
}