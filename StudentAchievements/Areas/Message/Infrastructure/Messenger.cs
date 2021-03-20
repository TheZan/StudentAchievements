using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using StudentAchievements.Models;
using StudentAchievements.Areas.Message.Models;
using StudentAchievements.Areas.Authorization.Models;

namespace StudentAchievements.Areas.Message.Infrastructure
{
    public class Messenger
    {
        private StudentAchievementsDbContext context;

        public Messenger(StudentAchievementsDbContext _context)
        {
            context = _context;
        }

        public async Task<bool> SendMessage(User receiver, User sender, string message)
        {
            if(receiver != null && sender != null && message != null)
            {
                if (!context.Chats.Any(p => (p.OneUser == receiver || p.OneUser == sender) && (p.TwoUser == receiver || p.TwoUser == sender)))
                {
                    await context.Chats.AddAsync(new Chat()
                    {
                        OneUser = receiver,
                        TwoUser = sender
                    });

                    await context.SaveChangesAsync();
                }

                await context.Messages.AddAsync(new Models.Message()
                {
                    MessageText = message,
                    Sender = sender.Name,
                    Chat = context.Chats.FirstOrDefault(p => (p.OneUser == receiver || p.OneUser == sender) && (p.TwoUser == receiver || p.TwoUser == sender)),
                    IsViewed = false,
                    SendDate = DateTime.Now
                });
                await context.SaveChangesAsync();

                return true;
            }

            return false;
        }

        public async Task<bool> CheckMessage(int id)
        {
            if(id != 0)
            {
                var message = context.Messages.FirstOrDefault(p => p.Id == id);
                if(message != null)
                {
                    message.IsViewed = true;

                    context.Messages.Update(message);
                    await context.SaveChangesAsync();

                    return true;
                }
            }

            return false;
        }

        public async Task<int> GetUnreadMessageCount(string userId)
        {
            int unreadMessageCount = 0;

            var currentUser = await context.Users.FirstOrDefaultAsync(p => p.Id == userId);

            var chatList = context.Chats.Include(m => m.Messages.Where(c => c.IsViewed == false && c.Sender != currentUser.Name)).Where(p => (p.OneUser == currentUser || p.TwoUser == currentUser)).ToList();
            
            foreach (var chat in chatList)
            {
                unreadMessageCount += chat.Messages.Count();
            }

            return unreadMessageCount;
        }
    }
}