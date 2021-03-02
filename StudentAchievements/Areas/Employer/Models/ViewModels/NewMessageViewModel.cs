using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using StudentAchievements.Areas.Authorization.Models;

namespace StudentAchievements.Areas.Employer.Models.ViewModels
{
    public class NewMessageViewModel
    {
        [Required]
        public string Message { get;set; }
        [Required]
        public string ReceiverId { get;set; }
        [Required]
        public string SenderId { get;set; }
    }
}