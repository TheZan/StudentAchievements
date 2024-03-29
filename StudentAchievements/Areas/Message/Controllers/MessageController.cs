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
using StudentAchievements.Areas.Message.Infrastructure;

namespace StudentAchievements.Areas.Message.Controllers
{
    [Area("Message")]
    [Authorize(Roles = "Employer, Student")]
    public class MessageController : Controller
    {
        private StudentAchievementsDbContext context;
        private IUserRepository userRepository;
        private Messenger messenger;
        
        private const int pageSize = 20;

        public MessageController(StudentAchievementsDbContext _context, IUserRepository _userRepository, Messenger _messenger)
        {
            context = _context;
            userRepository = _userRepository;
            messenger = _messenger;
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
        public async Task<IActionResult> GetMessages(string id, int? pageNumber)
        {
            var currentUser = await userRepository.Users.FirstOrDefaultAsync(u => u.Email == User.Identity.Name);

            var companion = await userRepository.Users.FirstOrDefaultAsync(u => u.Id == id);

            var numberOfMessageToskip = pageNumber * pageSize;

            var chat = await context.Chats.Include(o => o.OneUser)
                                        .Include(t => t.TwoUser)
                                        .Include(m => m.Messages.OrderByDescending(d => d.SendDate).Skip(Convert.ToInt32(numberOfMessageToskip)).Take(pageSize))
                                        .FirstOrDefaultAsync(p => (p.OneUser == currentUser || p.OneUser == companion) && (p.TwoUser == currentUser || p.TwoUser == companion));
            chat.Messages = chat.Messages.OrderBy(p => p.SendDate).ToList();

            var messageList = chat.Messages.Where(p => p.IsViewed == false).ToList();
            
            foreach (var message in messageList)
            {
                if(message.Sender == companion.Name)
                {
                    message.IsViewed = true;
                }
            }
            
            context.Messages.UpdateRange(messageList);
            await context.SaveChangesAsync();

            var model = new MessageListViewModel()
            {
                Me = currentUser,
                Companion = companion,
                Chat = chat
            };

            if(pageNumber == null)
            {
                return PartialView("MessageList", model);
            }
            else
            {
                return PartialView("Messages", model);
            }
        }

        [HttpPost]
        public async Task SendMessage(string receiverId, string message)
        {
            var sender = await userRepository.Users.FirstOrDefaultAsync(u => u.Email == User.Identity.Name);
            var receiver = await userRepository.Users.FirstOrDefaultAsync(u => u.Id == receiverId);

            await messenger.SendMessage(receiver, sender, message);
        }

        [HttpPost]
        public async Task CheckMessage(string id, string message)
        {
            if(id !="" && message != "")
            {
                var msgList = context.Messages.Include(c => c.Chat).Where(p => (p.Chat.OneUser.Id == id || p.Chat.TwoUser.Id == id) && p.MessageText == message).ToList();
                var msg = msgList.OrderBy(p => p.SendDate).LastOrDefault();
                
                if(msg != null)
                {
                    await messenger.CheckMessage(msg.Id);
                }
            }
        }

        [HttpGet]
        public async Task<int> GetMessageCount()
        {
            var currentUser = await userRepository.Users.FirstOrDefaultAsync(p => p.Email == User.Identity.Name);

            var count = await messenger.GetUnreadMessageCount(currentUser.Id);

            return count;
        }
    }
}