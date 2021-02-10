using Microsoft.AspNetCore.Http;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace StudentAchievements.Areas.Admin.Models.ViewModels
{
    public interface IAddUserViewModel
    {
        [Required]
        [DisplayName("ФИО")]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        [DisplayName("Email")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        [DisplayName("Фото")]
        public byte[] Photo { get; set; }

        [DisplayName("Добавить фото")]
        public IFormFile UploadPhoto { get; set; }
    }
}
