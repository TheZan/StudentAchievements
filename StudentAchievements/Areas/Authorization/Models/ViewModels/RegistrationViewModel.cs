using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace StudentAchievements.Areas.Authorization.Models.ViewModels
{
    public class RegistrationViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        [DisplayName("Логотип")]
        public IFormFile Photo { get; set; }
    }
}
