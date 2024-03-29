﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace StudentAchievements.Areas.Authorization.Models.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
