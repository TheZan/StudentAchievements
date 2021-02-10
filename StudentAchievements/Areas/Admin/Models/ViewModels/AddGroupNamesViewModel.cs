using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StudentAchievements.Areas.Admin.Models.ViewModels
{
    public class AddGroupNamesViewModel : IAddDataViewModel
    {
        [Required]
        [DisplayName("Название")]
        public string Name { get; set; }
    }
}
