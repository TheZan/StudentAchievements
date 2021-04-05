using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using StudentAchievements.Infrastructure;
using StudentAchievements.Areas.Admin.Models.ViewModels;


namespace StudentAchievements.Areas.Teacher.Models.ViewModels
{
    public class TeacherSettingsViewModel : IEditUserViewModel
    {
        public TeacherSettingsViewModel()
        {
            NotFoundUserPhoto = NotFoundImageUtility.GetNotFoundImage();
        }

        public string Id { get;set; }

        public string Name { get;set; }

        public string Email { get;set; }

        [Required]
        [DisplayName("Пол")]
        public string Gender { get;set; }

        [DisplayName("Фото")]
        public byte[] Photo { get; set; }

        [DisplayName("Фото")]
        public IFormFile UploadPhoto { get; set; }

        public byte[] NotFoundUserPhoto { get; set; }
        
        public DateTime Dob { get; set; }
    }
}