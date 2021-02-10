using Microsoft.AspNetCore.Http;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.IO;

namespace StudentAchievements.Areas.Admin.Models.ViewModels
{
    public class EmployerEditViewModel : IEditUserViewModel
    {
        public EmployerEditViewModel()
        {
            NotFoundUserPhoto = GetNotFoundImage();
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
