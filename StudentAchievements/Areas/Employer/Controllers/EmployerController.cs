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
using StudentAchievements.Models;
using StudentAchievements.Areas.Employer.Models.ViewModels;
using StudentAchievements.Areas.Message.Infrastructure;

namespace StudentAchievements.Areas.Employer.Controllers
{
    [Area("Employer")]
    [Authorize(Roles = "Employer")]
    public class EmployerController : Controller
    {
        private StudentAchievementsDbContext context;
        private IDataRepository dataRepository;
        private IUserRepository userRepository;
        private Messenger messenger;

        public EmployerController(StudentAchievementsDbContext _context, IDataRepository _dataRepository, IUserRepository _userRepository)
        {
            context = _context;
            dataRepository = _dataRepository;
            userRepository = _userRepository;
            messenger = new Messenger(context);
        }

        public IActionResult NewMessage() => PartialView();

        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, int? pageNumber)
        {
            ViewBag.StudentsSelected = "active";
            ViewBag.MessengerSelected = "";

            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["DobSortParm"] = sortOrder == "Dob" ? "dob_desc" : "Dob";
            ViewData["DirectionSortParm"] = sortOrder == "Direction" ? "direction_desc" : "Direction";
            ViewData["GenderSortParm"] = sortOrder == "Gender" ? "gender_desc" : "Gender";
            ViewData["GradeSortParm"] = sortOrder == "Grade" ? "grade_desc" : "Grade";
            ViewData["AchievementsSortParm"] = sortOrder == "Achievements" ? "achievements_desc" : "Achievements";

            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
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

            switch (sortOrder)
            {
                case "name_desc":
                    searchStudents = searchStudents.OrderByDescending(s => s.User.Name);
                    break;
                case "Dob":
                    searchStudents = searchStudents.OrderBy(s => s.Dob);
                    break;
                case "date_desc":
                    searchStudents = searchStudents.OrderByDescending(s => s.Dob);
                    break;
                case "Direction":
                    searchStudents = searchStudents.OrderBy(s => s.Group.Direction.Name);
                    break;
                case "direction_desc":
                    searchStudents = searchStudents.OrderByDescending(s => s.Group.Direction.Name);
                    break;
                case "Gender":
                    searchStudents = searchStudents.OrderBy(s => s.Gender);
                    break;
                case "gender_desc":
                    searchStudents = searchStudents.OrderByDescending(s => s.Gender);
                    break;
                case "Grade":
                    searchStudents = searchStudents.OrderBy(s => s.Group.Grade);
                    break;
                case "grade_desc":
                    searchStudents = searchStudents.OrderByDescending(s => s.Group.Grade);
                    break;
                case "Achievements":
                    searchStudents = searchStudents.OrderBy(s => s.Achievements.Count());
                    break;
                case "achievements_desc":
                    searchStudents = searchStudents.OrderByDescending(s => s.Achievements.Count());
                    break;
                default:
                    searchStudents = searchStudents.OrderBy(s => s.User.Name);
                    break;
            }

            int pageSize = 10;

            var studentList = await PaginatedList<StudentAchievements.Areas.Authorization.Models.Student>.CreateAsync(searchStudents.AsNoTracking(), pageNumber ?? 1, pageSize);

            var model = new StudentListViewModel(){ Students = studentList };

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> ViewStudentProfile(int? id)
        {
            var student = await userRepository.Students.Include(s => s.Achievements)
                                                        .Include(a => a.Assessments.OrderBy(o => o.Subject.Semester))
                                                        .ThenInclude(s => s.Subject)
                                                        .ThenInclude(c => c.ControlType)
                                                        .Include(f => f.FormEducation)
                                                        .Include(g => g.Group)
                                                        .ThenInclude(d => d.Direction)
                                                        .Include(u => u.User)
                                                        .FirstOrDefaultAsync(s => s.Id == id);

            foreach (var assesment in student.Assessments)
            {
                assesment.Score = await dataRepository.Scores.FirstOrDefaultAsync(p => p.Id == assesment.ScoreId);
            }

            var employer = await userRepository.Users.FirstOrDefaultAsync(u => u.Email == User.Identity.Name);

            if (student != null)
            {
                return View("StudentProfile", new StudentProfileViewModel()
                {
                    Student = student,
                    AssessmentsList = student.Assessments,
                    AchievementsList = student.Achievements,
                    NewMessage = new NewMessageViewModel()
                    {
                        ReceiverId = student.User.Id,
                        SenderId = employer.Id
                    } 
                });
            } 

            return StatusCode(StatusCodes.Status404NotFound);
        }

        [HttpPost]
        public async Task<IActionResult> SendMessage(NewMessageViewModel model)
        {
            var userOne = await userRepository.Users.FirstOrDefaultAsync(u => u.Id == model.ReceiverId.ToString());
            var userTwo = await userRepository.Users.FirstOrDefaultAsync(u => u.Id == model.SenderId.ToString());

            var student = await userRepository.Students.FirstOrDefaultAsync(p => p.User == userOne);

            await messenger.SendMessage(userOne, userTwo, model.Message);
            
            return RedirectToAction("ViewStudentProfile", new { id = student.Id });
        }
    }
}