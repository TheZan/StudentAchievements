using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Authorization;
using StudentAchievements.Areas.Authorization.Models;
using StudentAchievements.Areas.Message.Models;
using StudentAchievements.Areas.Message.Infrastructure;
using StudentAchievements.Models;

namespace StudentAchievements.Areas.Message.Infrastructure
{
    [Authorize(Roles = "Employer, Student")]
    public class ChatHub : Hub
    {
        private StudentAchievementsDbContext context;
        private IUserRepository userRepository;
        private Messenger messenger;

        public ChatHub(StudentAchievementsDbContext _context, IUserRepository _userRepository, Messenger _messenger)
        {
            context = _context;
            userRepository = _userRepository;
            messenger = _messenger;
        }

        public async Task Send(string message, string from, string to)
        {
            var sender = userRepository.Users.FirstOrDefault(s => s.Id == from);
            var receiver = userRepository.Users.FirstOrDefault(s => s.Id == to);
            await messenger.SendMessage(receiver, sender, message);
            
            await Clients.Others.SendAsync("Receive", from);
        }
    }
}