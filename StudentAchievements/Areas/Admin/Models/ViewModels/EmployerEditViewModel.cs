using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StudentAchievements.Areas.Admin.Models.ViewModels
{
    public class EmployerEditViewModel : IEditViewModel
    {
        [Required]
        [DisplayName("ФИО")]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        [DisplayName("Email")]
        public string Email { get; set; }

        public byte[] Photo { get; set; }

        [DisplayName("Описание")]
        public string Description { get; set; }
    }
}
