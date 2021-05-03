using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using StudentAchievements.Areas.Authorization.Models;
using StudentAchievements.Infrastructure;
using Microsoft.EntityFrameworkCore;
using StudentAchievements.Areas.Vacancies.Models;
using StudentAchievements.Models;
using Microsoft.AspNetCore.Http;
using StudentAchievements.Areas.Message.Infrastructure;
using StudentAchievements.Areas.Vacancies.Models.ViewModels;

namespace StudentAchievements.Areas.Vacancies.Controllers
{
    [Area("Vacancies")]
    [Authorize(Roles = "Employer, Student, Admin")]
    public class VacancyController : Controller
    {
        private IDataRepository dataRepository;
        private IUserRepository userRepository;
        private StudentAchievementsDbContext context;
        private Messenger messenger;

        public VacancyController(IDataRepository _dataRepository, StudentAchievementsDbContext _context, IUserRepository _userRepository)
        {
            dataRepository = _dataRepository;
            context = _context;
            messenger = new Messenger(context);
            userRepository = _userRepository;
        }

        public async Task<IActionResult> Index(string searchString, int? pageNumber)
        {
            if (searchString != null)
            {
                pageNumber = 1;
            }

            ViewData["CurrentFilter"] = searchString;

            var vacancies = dataRepository.Vacancies;
            if (!String.IsNullOrEmpty(searchString))
            {
                searchString = searchString.ToLower();

                vacancies = vacancies.Where(s => s.Name.ToLower().Contains(searchString)
                                       || s.Content.ToLower().Contains(searchString));
            }

            int pageSize = 10;

            return View(await PaginatedList<Vacancy>.CreateAsync(vacancies.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        [HttpGet]
        public async Task<IActionResult> ViewVacancy(int id)
        {
            var vacancy = await context.Vacancies.Include(e => e.Employer).ThenInclude(u => u.User).FirstOrDefaultAsync(p => p.Id == id);
            if (vacancy != null)
            {
                return View(vacancy);
            }

            return NotFound("Вакансия не найдена");
        }

        [HttpGet]
        [Authorize(Roles = "Employer")]
        public async Task<IActionResult> GetMyVacancy(string searchString, int? pageNumber)
        {
            var me = await userRepository.Users.FirstOrDefaultAsync(p => p.Email == User.Identity.Name);
            var employer = await context.Employers.Include(v => v.Vacancies).FirstOrDefaultAsync(p => p.User == me);
            if (employer != null)
            {
                if (searchString != null)
                {
                    pageNumber = 1;
                }

                ViewData["CurrentFilter"] = searchString;

                var vacancies = dataRepository.Vacancies.Where(e => e.EmployerId == employer.Id);
                if (!String.IsNullOrEmpty(searchString))
                {
                    searchString = searchString.ToLower();

                    vacancies = vacancies.Where(s => s.Name.ToLower().Contains(searchString)
                                           || s.Content.ToLower().Contains(searchString));
                }

                int pageSize = 10;

                return View("MyVacancy", await PaginatedList<Vacancy>.CreateAsync(vacancies.AsNoTracking(), pageNumber ?? 1, pageSize));
            }

            return NotFound("Вакансии не найдены");
        }

        [HttpGet]
        [Authorize(Roles = "Employer")]
        public IActionResult AddVacancy() => PartialView("NewVacancy");

        [HttpPost]
        [Authorize(Roles = "Employer")]
        public async Task<IActionResult> AddVacancy(NewVacancyViewModel model)
        {
            if (ModelState.IsValid)
            {
                var me = await userRepository.Employers.Include(u => u.User).FirstOrDefaultAsync(p => p.User.Email == User.Identity.Name);

                var vacancy = new Vacancy()
                {
                    Name = model.Name,
                    Salary = model.Salary,
                    Experience = model.Experience,
                    WorkType = model.WorkType,
                    WorkSchedule = model.WorkSchedule,
                    Content = model.Content,
                    Employer = me
                };

                await dataRepository.AddVacancy(vacancy);
            }

            return RedirectToAction("GetMyVacancy");
        }

        [HttpGet]
        [Authorize(Roles = "Employer, Admin")]
        public IActionResult ShowRemoveVacancy(int id) => PartialView("DeleteVacancy", id);

        [HttpPost]
        [Authorize(Roles = "Employer, Admin")]
        public async Task<IActionResult> RemoveVacancy(int id)
        {
            if (ModelState.IsValid)
            {
                var vacancy = await dataRepository.Vacancies.FirstOrDefaultAsync(p => p.Id == id);
                await dataRepository.DeleteVacancy(vacancy);
            }
            if (User.IsInRole("Employer"))
            {
                return RedirectToAction("GetMyVacancy");
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize(Roles = "Employer, Admin")]
        public async Task<IActionResult> EditVacancy(int id)
        {
            var vacancy = await dataRepository.Vacancies.FirstOrDefaultAsync(p => p.Id == id);
            var model = new EditVacancyViewModel()
            {
                Id = vacancy.Id,
                Name = vacancy.Name,
                Salary = vacancy.Salary,
                Experience = vacancy.Experience,
                WorkType = vacancy.WorkType,
                WorkSchedule = vacancy.WorkSchedule,
                Content = vacancy.Content
            };

            return PartialView("EditVacancy", model);
        }

        [HttpPost]
        [Authorize(Roles = "Employer, Admin")]
        public async Task<IActionResult> EditVacancy(EditVacancyViewModel model)
        {
            if (ModelState.IsValid)
            {
                var vacancy = await dataRepository.Vacancies.FirstOrDefaultAsync(p => p.Id == model.Id);
                vacancy.Name = model.Name;
                vacancy.Salary = model.Salary;
                vacancy.Experience = model.Experience;
                vacancy.WorkType = model.WorkType;
                vacancy.WorkSchedule = model.WorkSchedule;
                vacancy.Content = model.Content;

                await context.SaveChangesAsync();
            }

            if (User.IsInRole("Employer"))
            {
                return RedirectToAction("GetMyVacancy");
            }

            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Student")]
        public async Task<IActionResult> Respond(string id, string url, string name)
        {
            var userOne = await userRepository.Users.FirstOrDefaultAsync(u => u.Id == id);
            var userTwo = await userRepository.Users.FirstOrDefaultAsync(u => u.Email == User.Identity.Name);

            string message = $"Отклик на вакансию \"{name}\": https://{url}";


            await messenger.SendMessage(userOne, userTwo, message);

            return RedirectToAction("Index", "Message");
        }

        public IActionResult Back()
        {
            if (User.IsInRole("Employer"))
            {
                return RedirectToAction("Index", "Employer", new { area = "Employer" });
            }
            else if (User.IsInRole("Student"))
            {
                return RedirectToAction("Index", "Student", new { area = "Student" });
            }

            return RedirectToAction("Index", "Admin", new { area = "Admin" });
        }
    }
}