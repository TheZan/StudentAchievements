using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace StudentAchievements.Areas.Admin.Models.ViewModels
{
    public class EditUserViewModel
    {
        [Required]
        [DisplayName("ФИО")]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        [DisplayName("Email")]
        public string Email { get; set; }
    }
}
