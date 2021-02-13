using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StudentAchievements.Areas.Authorization.Models;
using StudentAchievements.Models;

namespace StudentAchievements.Areas.Admin.Models.ViewModels
{
    public class StudentEditViewModel : IEditUserViewModel
    {
        public StudentEditViewModel()
        {
            NotFoundUserPhoto = GetNotFoundImage();
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
        [DisplayName("Дата рождения")]
        public DateTime Dob { get; set; }

        [Required]
        [DisplayName("Учебная группа")]
        public int? Group { get; set; }

        [Required]
        [DisplayName("Форма обучения")]
        public int? FormEducation { get; set; }

        public IEnumerable<SelectListItem> GroupsList { get; set; }

        public IEnumerable<SelectListItem> FormEducationList { get; set; }

        private byte[] GetNotFoundImage()
        {
            byte[] photo = null;

            using (var stream = new FileStream($"{Directory.GetCurrentDirectory()}/wwwroot/favicons/notFoundUserPhoto.png", FileMode.Open, FileAccess.Read))
            {
                photo = new byte[stream.Length];

                using (var reader = new BinaryReader(stream))
                {
                    photo = reader.ReadBytes((int)stream.Length);
                }
            }

            return photo;
        }
    }
}
