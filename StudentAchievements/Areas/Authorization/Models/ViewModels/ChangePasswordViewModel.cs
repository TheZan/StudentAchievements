using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace StudentAchievements.Areas.Authorization.Models.ViewModels
{
    public class ChangePasswordViewModel
    {
        public string Id { get;set; }

        [Required]
        [DisplayName("Старый пароль")]
        [DataType(DataType.Password)]
        public string OldPassword { get; set; }

        [Required]
        [DisplayName("Новый пароль")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Required]
        [DisplayName("Подтверждение нового пароля")]
        [Compare("NewPassword", ErrorMessage = "Пароли не совпадают")]
        [DataType(DataType.Password)]
        public string ConfirmNewPassword { get; set; }
    }
}