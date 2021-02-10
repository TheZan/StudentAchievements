using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StudentAchievements.Areas.Authorization.Models;
using StudentAchievements.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StudentAchievements.Areas.Admin.Models.ViewModels
{
    public class AddStudentViewModel : IAddViewModel
    {
        [Required]
        [DisplayName("ФИО")]
        public string Name { get; set; }

        [Required]
        [DisplayName("Пол")]
        public string Gender { get; set; }

        [Required]
        [DisplayName("Дата рождения")]
        public DateTime Dob { get; set; }

        [Required]
        [EmailAddress]
        [DisplayName("Email")]
        public string Email { get; set; }

        [Required]
        [DisplayName("Пароль")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DisplayName("Подтверждение пароля")]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        [Required]
        [DisplayName("Учебная группа")]
        public int Group { get; set; }

        [Required]
        [DisplayName("Форма обучения")]
        public int FormEducation { get; set; }

        [DisplayName("Фото")]
        public byte[] Photo { get; set; }

        [DisplayName("Добавить фото")]
        public IFormFile UploadPhoto { get; set; }

        public IEnumerable<SelectListItem> GroupsList { get; set; }

        public IEnumerable<SelectListItem> FormEducationList { get; set; }
    }
}
