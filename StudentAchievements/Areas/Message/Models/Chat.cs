using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StudentAchievements.Areas.Authorization.Models;

namespace StudentAchievements.Areas.Message.Models
{
    public class Chat
    {
        public int Id { get;set; }
        public User OneUser { get;set; }
        public User TwoUser { get;set; }
        public List<Message> Messages { get;set; }
    }
}