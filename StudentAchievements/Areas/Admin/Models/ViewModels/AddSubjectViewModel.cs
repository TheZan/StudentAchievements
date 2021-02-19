using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using StudentAchievements.Areas.Authorization.Models;

namespace StudentAchievements.Areas.Admin.Models.ViewModels
{
    public class AddSubjectViewModel
    {
        public int Id { get; set; }

        [Required]
        [DisplayName("Название")]
        public string Name { get; set; }
        [Required]
        [DisplayName("Курс")]
        [Range(1, Int32.MaxValue)]
        public int Grade { get; set; }
        [Required]
        [DisplayName("Семестр")]
        [Range(1, Int32.MaxValue)]
        public int Semester { get; set; }

        [Required]
        [DisplayName("Вид контроля")]
        public int? ControlType { get; set; }

        public IEnumerable<SelectListItem> ControlTypeList { get; set; }
    }
}
