using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace StudentAchievements.Areas.Admin.Models.ViewModels
{
    public class EditDirectionViewModel : IEditDataViewModel
    {
        public int Id { get; set; }

        [Required]
        [DisplayName("Название")]
        public string Name { get; set; }

        [Required]
        [DisplayName("Название группы")]
        public string GroupName { get; set; }

        [Required]
        [DisplayName("Факультет")]
        public int? Department { get; set; }

        public IEnumerable<SelectListItem> DepartmentsList { get; set; }

        [Required]
        [DisplayName("Тип обучения")]
        public int? ProgramType { get; set; }

        public IEnumerable<SelectListItem> ProgramTypeList { get; set; }
    }
}