using StudentAchievements.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentAchievements.Areas.Admin.Models.ViewModels
{
    public class AddDataViewModel
    {
        private StudentAchievementsDbContext context;

        public AddDataViewModel(StudentAchievementsDbContext _context)
        {
            context = _context;

            AddDepartmentsViewModel = new AddDepartmentsViewModel()
            {
                Departments = context.Departments
            };
        }

        public AddDepartmentsViewModel AddDepartmentsViewModel { get; set; }
    }
}
