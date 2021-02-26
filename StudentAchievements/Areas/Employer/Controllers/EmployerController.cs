using System.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StudentAchievements.Areas.Employer;
using StudentAchievements.Areas.Authorization.Models;
using StudentAchievements.Infrastructure;
using StudentAchievements.Areas.Employer.Models.ViewModels;

namespace StudentAchievements.Areas.Employer.Controllers
{
    [Area("Employer")]
    [Authorize(Roles = "Employer")]
    public class EmployerController : Controller
    {
        private IDataRepository dataRepository;
        private IUserRepository userRepository;

        public EmployerController(IDataRepository _dataRepository, IUserRepository _userRepository)
        {
            dataRepository = _dataRepository;
            userRepository = _userRepository;
        }

        public async Task<IActionResult> Index(string searchString, int? pageNumber)
        {
            ViewBag.StudentsSelected = "active";
            ViewBag.MessengerSelected = "";

            if (searchString != null)
            {
                pageNumber = 1;
            }

            ViewData["CurrentFilter"] = searchString;

            IQueryable<StudentAchievements.Areas.Authorization.Models.Student> searchStudents = userRepository.Students.Include(g => g.Group)
                                                                                                                        .ThenInclude(d => d.Direction)
                                                                                                                        .Include(u => u.User)
                                                                                                                        .Include(f => f.FormEducation)
                                                                                                                        .Include(a => a.Achievements)
                                                                                                                        .Include(a => a.Assessments)
                                                                                                                        .ThenInclude(s => s.Score);

             

            if (!String.IsNullOrEmpty(searchString))
            {
                searchStudents = searchStudents.Where(s => s.User.Name.Contains(searchString)
                                       || s.User.Email.Contains(searchString)
                                       || s.Group.Direction.Name.Contains(searchString));
            }

            int pageSize = 10;

            var studentList = await PaginatedList<StudentAchievements.Areas.Authorization.Models.Student>.CreateAsync(searchStudents.AsNoTracking(), pageNumber ?? 1, pageSize);

            var model = new StudentListViewModel(){ Students = studentList };

            return View(model);
        }
    }
}