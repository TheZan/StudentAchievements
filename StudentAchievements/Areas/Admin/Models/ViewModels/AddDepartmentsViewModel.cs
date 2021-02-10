using StudentAchievements.Areas.Authorization.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StudentAchievements.Areas.Admin.Models.ViewModels
{
    public class AddDepartmentsViewModel : IAddDataViewModel
    {
        [Required]
        [DisplayName("Название")]
        public string Name { get; set; }

        public IEnumerable<Department> Departments { get; set; }
    }
}
