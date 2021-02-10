using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StudentAchievements.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentAchievements.Areas.Admin.Models.ViewModels
{
    public class AddUsersViewModel
    {
        private StudentAchievementsDbContext context;

        public AddUsersViewModel(StudentAchievementsDbContext _context)
        {
            context = _context;

            AddStudentViewModel = new AddStudentViewModel()
            {
                GroupsList = context.Groups.Include(g => g.Name).Select(p => new SelectListItem { Value = p.Id.ToString(), Text = $"{p.Name.Name}-{p.Number}" }),
                FormEducationList = context.FormEducations.Select(p => new SelectListItem { Value = p.Id.ToString(), Text = p.Name })
            };

            AddTeacherViewModel = new AddTeacherViewModel()
            {
                DepartmentsList = context.Departments.Select(p => new SelectListItem { Value = p.Id.ToString(), Text = p.Name })
            };
        }

        public AddStudentViewModel AddStudentViewModel { get; set; }
        public AddTeacherViewModel AddTeacherViewModel { get; set; }
    }
}
