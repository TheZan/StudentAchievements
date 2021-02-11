using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StudentAchievements.Models;
using System.Linq;

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

            AddDirectionsViewModel = new AddDirectionsViewModel()
            {
                Directions = context.Directions.Include(d => d.Department).Include(p => p.ProgramType),
                DepartmentsList = context.Departments.Select(p => new SelectListItem { Value = p.Id.ToString(), Text = p.Name }),
                ProgramTypeList = context.ProgramType.Select(p => new SelectListItem { Value = p.Id.ToString(), Text = p.Name })
            };
        }

        public AddDepartmentsViewModel AddDepartmentsViewModel { get; set; }
        public AddDirectionsViewModel AddDirectionsViewModel { get; set; }
    }
}
