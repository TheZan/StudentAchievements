using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
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

            var assessments = student.Assessments;

            if (student != null)
            {
                var assessmentsList = new List<AssessmentViewModel>();

                foreach (var assessment in assessments)
                {
                    List<SelectListItem> scoreList = new List<SelectListItem>();

                    switch (assessment.Subject.ControlType.Name)
                    {
                        case "Зачет":
                            scoreList.AddRange(dataRepository.Scores.Where(s => s.Name == "Зачет" || s.Name == "Нет оценки").Select(p => new SelectListItem() { Value = p.Id.ToString(), Text = p.Name }));
                            break;
                        case "Дифференцированный зачет":
                            scoreList.AddRange(dataRepository.Scores.Where(s => s.Name == "Удовлетворительно" || s.Name == "Хорошо" || s.Name == "Отлично" || s.Name == "Нет оценки").Select(p => new SelectListItem() { Value = p.Id.ToString(), Text = p.Name }));
                            break;
                        case "Экзамен":
                            scoreList.AddRange(dataRepository.Scores.Where(s => s.Name == "Удовлетворительно" || s.Name == "Хорошо" || s.Name == "Отлично" || s.Name == "Нет оценки").Select(p => new SelectListItem() { Value = p.Id.ToString(), Text = p.Name }));
                            break;
                    }

                    assessmentsList.Add(new AssessmentViewModel()
                    {
                        StudentId = student.Id,
                        AssessmentId = assessment.Id,
                        SubjectName = assessment.Subject.Name,
                        SubjectGrade = assessment.Subject.Grade,
                        SubjectSemester = assessment.Subject.Semester,
                        ControlTypeName = assessment.Subject.ControlType.Name,
                        Score = assessment.ScoreId,
                        ScoreList = scoreList
                    });
                }

                return View(new StudentProfileViewModel()
                {
                    Student = student,
                    AssessmentsList = assessmentsList,
                    AchievementViewModel = new AchievementViewModel()
                    {
                        Achievements = student.Achievements,
                        StudentId = student.Id
                    }
                });
            }

            return StatusCode(StatusCodes.Status404NotFound);
        }

        public IActionResult AssessmentsTable() => PartialView("AssessmentsTable");

        public IActionResult AchievementsTable() => PartialView("AchievementsTable");

        [HttpPost]
        public async Task<IActionResult> ViewStudentProfile(IList<AssessmentViewModel> model)
        {
            if (model != null)
            {
                await userRepository.EditStudentAssessments(model);
            }

            return RedirectToAction(nameof(ViewStudentProfile), new { id = model[0].StudentId });
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

            return RedirectToAction(nameof(ViewStudentProfile), new { id = model.StudentId });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteAchievement(int id, int studentId)
        {
            var achievement = dataRepository.Achievements.FirstOrDefault(d => d.Id == id);

            if (achievement != null)
            {
                await dataRepository.DeleteAchievement(achievement);
            }

            return RedirectToAction(nameof(ViewStudentProfile), new { id = studentId });
        }
    }
}
