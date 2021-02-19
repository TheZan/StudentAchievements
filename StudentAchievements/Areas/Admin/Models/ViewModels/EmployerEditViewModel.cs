﻿using Microsoft.AspNetCore.Http;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using StudentAchievements.Infrastructure;

namespace StudentAchievements.Areas.Admin.Models.ViewModels
{
    public class EmployerEditViewModel : IEditUserViewModel
    {
        public EmployerEditViewModel()
        {
            NotFoundUserPhoto = NotFoundImageUtility.GetNotFoundImage();
        }

        public string Id { get; set; }

        [Required]
        [DisplayName("ФИО")]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        [DisplayName("Email")]
        public string Email { get; set; }

        [DisplayName("Фото")]
        public byte[] Photo { get; set; }

        [DisplayName("Изменить фото")]
        public IFormFile UploadPhoto { get; set; }

        public byte[] NotFoundUserPhoto { get; set; }

        [DisplayName("Описание")]
        public string Description { get; set; }
    }
}
