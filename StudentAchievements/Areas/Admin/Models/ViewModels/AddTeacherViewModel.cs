using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StudentAchievements.Areas.Admin.Models.ViewModels
{
    public class AddTeacherViewModel : IAddUserViewModel
    {
        [Required]
        [DisplayName("ФИО")]
        public string Name { get; set; }

        [Required]
        [DisplayName("Пол")]
        public string Gender { get; set; }

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
        [DisplayName("Факультет")]
        public int Department { get; set; }

        [DisplayName("Фото")]
        public byte[] Photo { get; set; }

        [DisplayName("Добавить фото")]
        public IFormFile UploadPhoto { get; set; }

        public IEnumerable<SelectListItem> DepartmentsList { get; set; }
    }
}
