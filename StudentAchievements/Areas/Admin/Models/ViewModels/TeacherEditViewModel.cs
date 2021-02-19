using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using StudentAchievements.Areas.Authorization.Models;
using StudentAchievements.Infrastructure;
using StudentAchievements.Models;

namespace StudentAchievements.Areas.Admin.Models.ViewModels
{
    public class TeacherEditViewModel : IEditUserViewModel
    {
        public TeacherEditViewModel()
        {
            NotFoundUserPhoto = NotFoundImageUtility.GetNotFoundImage();
        }

        public string Id { get; set; }

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

        [DisplayName("Фото")]
        public byte[] Photo { get; set; }

        [DisplayName("Изменить фото")]
        public IFormFile UploadPhoto { get; set; }

        public byte[] NotFoundUserPhoto { get; set; }

        [Required]
        [DisplayName("Факультет")]
        public int? Department { get; set; }

        public IEnumerable<SelectListItem> DepartmentsList { get; set; }
    }
}
