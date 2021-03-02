using System;
using System.ComponentModel.DataAnnotations;
using StudentAchievements.Areas.Authorization.Models;

namespace StudentAchievements.Areas.Message.Models
{
    public class Message
    {
        public int Id { get;set; }
        public string MessageText { get;set; }
        public string Sender { get;set; }
        public int ChatId { get;set; }
        public Chat Chat { get;set; }
        public bool IsViewed { get;set; }
        public DateTime SendDate { get;set; }
    }
}