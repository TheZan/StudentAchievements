using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace StudentAchievements.Areas.Admin.Models.ViewModels
{
    public class EditGroupViewModel : IEditDataViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }

        [Required]
        [DisplayName("Номер")]
        public int Number { get; set; }

        [Required]
        [DisplayName("Направление")]
        public int? Direction { get; set; }

        public IEnumerable<SelectListItem> DirectionsList { get; set; }
    }
}
