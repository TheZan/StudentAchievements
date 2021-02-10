using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace StudentAchievements.Areas.Admin.Models.ViewModels
{
    public class AddDirectionsViewModel : IAddDataViewModel
    {
        [Required]
        [DisplayName("Название")]
        public string Name { get; set; }
    }
}
