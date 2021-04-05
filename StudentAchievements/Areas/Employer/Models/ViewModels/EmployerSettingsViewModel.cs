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

namespace StudentAchievements.Areas.Employer.Models.ViewModels
{
    public class EmployerSettingsViewModel : IEditUserViewModel
    {
        public EmployerSettingsViewModel()
        {
            NotFoundUserPhoto = NotFoundImageUtility.GetNotFoundImage();
        }

        public string Id { get;set; }

        public string Name { get;set; }

        public string Email { get;set; }

        public string Gender { get;set; }

        [Required]
        [DisplayName("Описание")]
        public string Description { get;set; }

        [DisplayName("Фото")]
        public byte[] Photo { get; set; }

        [DisplayName("Фото")]
        public IFormFile UploadPhoto { get; set; }

        public byte[] NotFoundUserPhoto { get; set; }

        public DateTime Dob { get; set; }
    }
}