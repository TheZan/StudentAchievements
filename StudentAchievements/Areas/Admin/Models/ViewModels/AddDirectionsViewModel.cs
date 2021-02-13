using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using DynamicVML;
using StudentAchievements.Areas.Authorization.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace StudentAchievements.Areas.Admin.Models.ViewModels
{
    public class AddDirectionsViewModel : IAddDataViewModel
    {
        public int Id { get; set; }

        [Required]
        [DisplayName("Название")]
        public string Name { get; set; }

        [Required]
        [DisplayName("Название группы")]
        public string GroupName { get; set; }

        public IEnumerable<Direction> Directions { get; set; }

        [Required]
        [DisplayName("Факультет")]
        public int? Department { get; set; }

        public IEnumerable<SelectListItem> DepartmentsList { get; set; }

        [Required]
        [DisplayName("Тип обучения")]
        public int? ProgramType { get; set; }

        public IEnumerable<SelectListItem> ProgramTypeList { get; set; }

        public DynamicList<Subject> SubjectsList { get; set; } = new DynamicList<Subject>();
    }
}
