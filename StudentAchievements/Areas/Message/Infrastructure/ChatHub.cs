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
using StudentAchievements.Infrastructure;

namespace StudentAchievements.Areas.Message.Infrastructure
{
    [Authorize(Roles = "Employer, Student")]
    public class ChatHub : Hub
    {
        private StudentAchievementsDbContext context;
        private IUserRepository userRepository;
        private Messenger messenger;
        private ConnectionProvider connectionProvider;

        public ChatHub(StudentAchievementsDbContext _context, IUserRepository _userRepository, Messenger _messenger, ConnectionProvider _connectionProvider)
        {
            context = _context;
            userRepository = _userRepository;
            messenger = _messenger;
            connectionProvider = _connectionProvider;
        }

        public async Task Send(string message, string from, string to)
        {
            var sender = userRepository.Users.FirstOrDefault(s => s.Id == from);
            var receiver = userRepository.Users.FirstOrDefault(s => s.Id == to);
            await messenger.SendMessage(receiver, sender, message);
            var sendDate = DateTime.Now;

            var userPhoto = GetPhotoSender(sender);

            string senderPhoto = String.Empty;

            if (!String.IsNullOrEmpty(userPhoto))
            {
                senderPhoto = userPhoto;
            }
            else
            {
                senderPhoto = Convert.ToBase64String(NotFoundImageUtility.GetNotFoundImage());
            }

            if(!String.IsNullOrEmpty(connectionProvider.GetConnection(receiver.Id)))
            {
                await Clients.Client(connectionProvider.GetConnection(receiver.Id)).SendAsync("Receive", message, sender.Name, from, sendDate.ToString("dd/MM/yyyy, HH:mm:ss"), senderPhoto);
            }
        }

        private string GetPhotoSender(User sender)
        {
            string senderPhoto = String.Empty;

            if(sender.Photo != null)
            {
                senderPhoto = Convert.ToBase64String(sender.Photo);
            }

            return senderPhoto;
        }

        private string GetPhotoReceiver(User receiver)
        {
            string receiverPhoto = String.Empty;

            if(receiver.Photo != null)
            {
                receiverPhoto = Convert.ToBase64String(receiver.Photo);
            }

            return receiverPhoto;
        }

        public override async Task OnConnectedAsync()
        {
            var currentUser = userRepository.Users.FirstOrDefault(u => u.Email == Context.User.Identity.Name);
            connectionProvider.AddConnection(currentUser.Id, Context.ConnectionId);

            var userPhoto = GetPhotoReceiver(currentUser);

            string receiverPhoto = String.Empty;

            if (!String.IsNullOrEmpty(userPhoto))
            {
                receiverPhoto = userPhoto;
            }
            else
            {
                receiverPhoto = Convert.ToBase64String(NotFoundImageUtility.GetNotFoundImage());
            }
            
            await base.OnConnectedAsync();
            await Clients.Caller.SendAsync("Connected", receiverPhoto);
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            var currentUserId = userRepository.Users.FirstOrDefault(u => u.Email == Context.User.Identity.Name).Id;
            connectionProvider.RemoveConnection(currentUserId);
            return base.OnDisconnectedAsync(exception);
        }
    }
}