using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using StudentAchievements.Areas.Authorization.Models;
using StudentAchievements.Areas.Teacher.Models.ViewModels;

namespace StudentAchievements.Areas.Teacher.Controllers
{
    [Area("Teacher")]
    [Authorize(Roles = "Teacher")]
    public class TeacherController : Controller
    {
        private IDataRepository dataRepository;
        private IUserRepository userRepository;

        public TeacherController(IDataRepository _dataRepository, IUserRepository _userRepository)
        {
            dataRepository = _dataRepository;
            userRepository = _userRepository;
        }

        public ViewResult Index()
        {
            ViewBag.StudentsSelected = "active";
            
            var currentTeacher = userRepository.Teachers.FirstOrDefault(t => t.User.Email == User.Identity.Name);

            var t = dataRepository.Groups.Include(s => s.Students).ThenInclude(u => u.User)
                    .Include(d => d.Direction).Where(p => p.Direction.DepartmentId == currentTeacher.DepartmentId);

            return View(new StudentListViewModel()
            {
                Directions = dataRepository.Directions.Include(g => g.Groups)
                                                        .ThenInclude(s => s.Students)
                                                        .ThenInclude(u => u.User)
                                                        .Where(d => d.DepartmentId == currentTeacher.DepartmentId)
            });
        }
    }
}
