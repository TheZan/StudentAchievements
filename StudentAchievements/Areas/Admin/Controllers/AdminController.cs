using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StudentAchievements.Areas.Admin.Models.ViewModels;
using StudentAchievements.Areas.Authorization.Models;
using StudentAchievements.Models;

namespace StudentAchievements.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private IdentityDbContext context;
        private UserManager<User> userManager;
        private IUserRepository repository;

        public AdminController(IUserRepository _repository, UserManager<User> _userManager, IdentityDbContext _context)
        {
            context = _context;
            repository = _repository;
            userManager = _userManager;
        }

        public ViewResult Index()
        {
            ViewBag.UsersSelected = "active";
            ViewBag.AboutSelected = "";
            ViewBag.AddUsersSelected = "";

            return View(GetUsers(1));
        }

        [HttpPost]
        public ViewResult Index(int currentPageIndex)
        {
            ViewBag.UsersSelected = "active";
            ViewBag.AboutSelected = "";
            ViewBag.AddUsersSelected = "";

            return View(GetUsers(currentPageIndex));
        }

        public ViewResult About()
        {
            ViewBag.UsersSelected = "";
            ViewBag.AboutSelected = "active";
            ViewBag.AddUsersSelected = "";

            return View();
        }

        public ViewResult AddUsers()
        {
            ViewBag.UsersSelected = "";
            ViewBag.AboutSelected = "";
            ViewBag.AddUsersSelected = "active";

            return View();
        }

        private UsersListViewModel GetUsers(int currentPage)
        {
            int maxRows = 10;
            var usersModel = new UsersListViewModel(userManager)
            {
                Users = repository.Users
                    .OrderBy(user => user.Id)
                    .Skip((currentPage - 1) * maxRows)
                    .Take(maxRows).ToList()
            };

            //if (string.IsNullOrWhiteSpace(searchString))
            //{

            //    usersModel.ApplicationUsers = repository.ApplicationUsers
            //        .OrderBy(user => user.Id)
            //        .Skip((currentPage - 1) * maxRows)
            //        .Take(maxRows).ToList();
            //    usersModel.IdentityUsers = repository.IdentityUsers;
            //}
            //else
            //{
            //    usersModel.ApplicationUsers = repository.ApplicationUsers
            //        .Where(u => u.Name.Contains(searchString))
            //        .OrderBy(user => user.Id)
            //        .Skip((currentPage - 1) * maxRows)
            //        .Take(maxRows).ToList();
            //    usersModel.IdentityUsers = repository.IdentityUsers;
            //}

            double pageCount = (double)(repository.Users.Count() / Convert.ToDecimal(maxRows));
            usersModel.PageCount = (int)Math.Ceiling(pageCount);

            usersModel.CurrentPageIndex = currentPage;

            return usersModel;
        }

        [HttpGet]
        public async Task<IActionResult> EditUser(string id)
        {
            var user = await userManager.FindByIdAsync(id);

            if (user != null)
            {
                var userRole = await userManager.GetRolesAsync(user);
                switch (userRole.First())
                {
                    case "Admin":
                        var adminInfo = context.Administrators.FirstOrDefault(u => u.User.Id == user.Id);
                        var adminModel = new AdminEditViewModel()
                        {
                            Email = adminInfo.User.Email,
                            Name = adminInfo.User.Name,
                            Photo = adminInfo.User.Photo
                        };
                        return View("EditAdmin", adminModel);
                    case "Employer":
                        var employerInfo = context.Employers.FirstOrDefault(u => u.User.Id == user.Id);
                        var employerModel = new EmployerEditViewModel()
                        {
                            Email = employerInfo.User.Email,
                            Name = employerInfo.User.Name,
                            Photo = employerInfo.User.Photo,
                            Description = employerInfo.Description
                        };
                        return View("EditEmployer", employerModel);
                    case "Teacher":
                        var teacherInfo = context.Teachers.FirstOrDefault(u => u.User.Id == user.Id);
                        var teacherModel = new TeacherEditViewModel(context)
                        {
                            Email = teacherInfo.User.Email,
                            Name = teacherInfo.User.Name,
                            Photo = teacherInfo.User.Photo,
                            Department = teacherInfo.Department
                        };
                        return View("EditTeacher", teacherModel);
                    case "Student":
                        var studentInfo = context.Students.FirstOrDefault(u => u.User.Id == user.Id);
                        var studentModel = new StudentEditViewModel(context)
                        {
                            Email = studentInfo.User.Email,
                            Name = studentInfo.User.Name,
                            Photo = studentInfo.User.Photo,
                            Dob = studentInfo.Dob,
                            Group = studentInfo.Group
                        };
                        return View("EditStudent", studentModel);
                }
            }
            else
            {
                ModelState.AddModelError("", "Пользователь не найден.");
            }

            return StatusCode(StatusCodes.Status404NotFound);
        }

        [HttpPost]
        public async Task<IActionResult> EditAdmin(AdminEditViewModel model)
        {
            var user = await userManager.FindByEmailAsync(model.Email);

            if (user != null)
            {
                var result = await repository.EditUser(user, model);

                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(Index));
                }

                return View("EditAdmin", model);
            }
            else
            {
                ModelState.AddModelError("", "Пользователь не найден.");
            }

            return StatusCode(StatusCodes.Status404NotFound);
        }

        [HttpPost]
        public async Task<IActionResult> EditEmployer(EmployerEditViewModel model)
        {
            var user = await userManager.FindByEmailAsync(model.Email);

            if (user != null)
            {
                var result = await repository.EditUser(user, model);

                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(Index));
                }

                return View("EditEmployer", model);
            }
            else
            {
                ModelState.AddModelError("", "Пользователь не найден.");
            }

            return StatusCode(StatusCodes.Status404NotFound);
        }

        [HttpPost]
        public async Task<IActionResult> EditTeacher(TeacherEditViewModel model)
        {
            var user = await userManager.FindByEmailAsync(model.Email);

            if (user != null)
            {
                var result = await repository.EditUser(user, model);

                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(Index));
                }

                return View("EditTeacher", model);
            }
            else
            {
                ModelState.AddModelError("", "Пользователь не найден.");
            }

            return StatusCode(StatusCodes.Status404NotFound);
        }

        [HttpPost]
        public async Task<IActionResult> EditStudent(StudentEditViewModel model)
        {
            var user = await userManager.FindByEmailAsync(model.Email);

            if (user != null)
            {
                var result = await repository.EditUser(user, model);

                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(Index));
                }

                return View("EditStudent", model);
            }
            else
            {
                ModelState.AddModelError("", "Пользователь не найден.");
            }

            return StatusCode(StatusCodes.Status404NotFound);
        }
    }
}