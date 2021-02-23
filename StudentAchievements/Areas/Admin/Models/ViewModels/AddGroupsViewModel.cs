using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using StudentAchievements.Areas.Authorization.Models;

namespace StudentAchievements.Areas.Admin.Models.ViewModels
{
    public class AddGroupsViewModel : IAddDataViewModel
    {
        [Required]
        [DisplayName("Год обучения")]
        public int Grade { get; set; }

        [Required]
        [DisplayName("Номер")]
        public int Number { get; set; }

        public IEnumerable<Group> Groups { get; set; }

        [Required]
        [DisplayName("Направление")]
        public int? Direction { get; set; }

        public IEnumerable<SelectListItem> DirectionsList { get; set; }

        public string Name { get; set; }
    }
}
