using System.ComponentModel.DataAnnotations;

namespace StudentAchievements.Areas.Authorization.Models.ViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
