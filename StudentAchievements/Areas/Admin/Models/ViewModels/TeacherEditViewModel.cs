using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using StudentAchievements.Areas.Authorization.Models;

namespace StudentAchievements.Areas.Admin.Models.ViewModels
{
    public class TeacherEditViewModel : IEditViewModel
    {
        [Required]
        [DisplayName("ФИО")]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        [DisplayName("Email")]
        public string Email { get; set; }

        public byte[] Photo { get; set; }

        [Required]
        [DisplayName("Факультет")]
        public Department Department { get; set; }
    }
}
