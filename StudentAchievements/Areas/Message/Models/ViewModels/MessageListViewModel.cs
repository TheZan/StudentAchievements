using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StudentAchievements.Areas.Authorization.Models;

namespace StudentAchievements.Areas.Message.Models.ViewModels
{
    public class MessageListViewModel
    {
        public User Me { get;set; }
        public User Companion { get;set; }
        public Chat Chat { get;set; }
        public int UnreadMessageCount { get;set; }
    }
}