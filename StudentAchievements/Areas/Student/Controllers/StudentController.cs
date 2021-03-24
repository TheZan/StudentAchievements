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
using StudentAchievements.Areas.Student.Models.ViewModels;
using StudentAchievements.Areas.Message.Infrastructure;
using StudentAchievements.Areas.Teacher.Models.ViewModels;

namespace StudentAchievements.Areas.Student.Controllers
{
    [Area("Student")]
    [Authorize(Roles = "Student")]
    public class StudentController : Controller
    {
        private IUserRepository userRepository;
        private IDataRepository dataRepository;

        public StudentController(IUserRepository _userRepository, IDataRepository _dataRepository)
        {
            userRepository = _userRepository;
            dataRepository = _dataRepository;
        }

        public IActionResult AchievementsTable() => PartialView("AchievementsTable");

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var student = await userRepository.Students.Include(s => s.Achievements)
                                                        .Include(a => a.Assessments.OrderBy(o => o.Subject.Semester))
                                                        .ThenInclude(s => s.Subject)
                                                        .ThenInclude(c => c.ControlType)
                                                        .Include(f => f.FormEducation)
                                                        .Include(g => g.Group)
                                                        .ThenInclude(d => d.Direction)
                                                        .Include(u => u.User)
                                                        .FirstOrDefaultAsync(s => s.User.Email == User.Identity.Name);

            foreach (var assesment in student.Assessments)
            {
                assesment.Score = await dataRepository.Scores.FirstOrDefaultAsync(p => p.Id == assesment.ScoreId);
            }

            if (student != null)
            {
                return View("Index", new StudentAchievements.Areas.Student.Models.ViewModels.StudentProfileViewModel()
                {
                    Student = student,
                    AssessmentsList = student.Assessments,
                    AchievementViewModel = new Teacher.Models.ViewModels.AchievementViewModel(){ Achievements = student.Achievements, StudentId = student.Id }
                });
            } 

            return StatusCode(StatusCodes.Status404NotFound);
        }

        [HttpPost]
        public async Task<IActionResult> AddAchievement(AchievementViewModel model)
        {
            if (ModelState.IsValid)
            {
                await dataRepository.AddAchievement(new Achievement()
                {
                    Name = model.Name,
                    Description = model.Description,
                    Student = await userRepository.Students.FirstOrDefaultAsync(s => s.Id == model.StudentId)
                });
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> DeleteAchievement(int id, int studentId)
        {
            var achievement = dataRepository.Achievements.FirstOrDefault(d => d.Id == id);

            if (achievement != null)
            {
                await dataRepository.DeleteAchievement(achievement);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}