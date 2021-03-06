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

        public ChatHub(StudentAchievementsDbContext _context, IUserRepository _userRepository)
        {
            context = _context;
            userRepository = _userRepository;
        }

        public async Task Send(string message, string to)
        {
            var userInfoReciever = userRepository.Users.FirstOrDefault(p => p.Email == to).Id;
            await Clients.User(userInfoReciever).SendAsync("Send", message);
        }
    }
}