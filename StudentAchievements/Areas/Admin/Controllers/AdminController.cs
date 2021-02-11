using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StudentAchievements.Areas.Admin.Models.ViewModels;
using StudentAchievements.Areas.Authorization.Models;
using StudentAchievements.Infrastructure;
using StudentAchievements.Models;

namespace StudentAchievements.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private StudentAchievementsDbContext context;
        private UserManager<User> userManager;
        private IUserRepository repository;
        private IDataRepository dataRepository;

        public AdminController(IUserRepository _repository, UserManager<User> _userManager, StudentAchievementsDbContext _context, IDataRepository _dataRepository)
        {
            context = _context;
            repository = _repository;
            userManager = _userManager;
            dataRepository = _dataRepository;
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

            return View(new AddUsersViewModel(context));
        }

        public ViewResult AddData()
        {
            ViewBag.UsersSelected = "";
            ViewBag.AboutSelected = "";
            ViewBag.AddUsersSelected = "";
            ViewBag.DataSelected = "active";

            return View(new AddDataViewModel(context));
        }

        public IActionResult AddStudents() => PartialView("AddStudents");

        public IActionResult AddTeachers() => PartialView("AddTeachers");

        public IActionResult AddDepartments() => PartialView("AddDepartments");

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
                            Photo = adminInfo.User.Photo,
                            Gender = adminInfo.Gender,
                            Id = id
                        };
                        return View("EditAdmin", adminModel);
                    case "Employer":
                        var employerInfo = context.Employers.FirstOrDefault(u => u.User.Id == user.Id);
                        var employerModel = new EmployerEditViewModel()
                        {
                            Email = employerInfo.User.Email,
                            Name = employerInfo.User.Name,
                            Photo = employerInfo.User.Photo,
                            Description = employerInfo.Description,
                            Id = id
                        };
                        return View("EditEmployer", employerModel);
                    case "Teacher":
                        var teacherInfo = context.Teachers.Include(d => d.Department).FirstOrDefault(u => u.User.Id == user.Id);
                        var teacherModel = new TeacherEditViewModel()
                        {
                            Email = teacherInfo.User.Email,
                            Name = teacherInfo.User.Name,
                            Photo = teacherInfo.User.Photo,
                            Gender = teacherInfo.Gender,
                            Department = teacherInfo.Department.Id,
                            DepartmentsList = context.Departments.Select(p => new SelectListItem { Value = p.Id.ToString(), Text = p.Name }),
                            Id = id
                        };
                        return View("EditTeacher", teacherModel);
                    case "Student":
                        var studentInfo = context.Students.Include(s => s.Group).Include(f => f.FormEducation).FirstOrDefault(u => u.User.Id == user.Id);
                        var studentModel = new StudentEditViewModel()
                        {
                            Email = studentInfo.User.Email,
                            Name = studentInfo.User.Name,
                            Photo = studentInfo.User.Photo,
                            Dob = studentInfo.Dob,
                            Group = studentInfo.Group.Id,
                            Gender = studentInfo.Gender,
                            FormEducation = studentInfo.FormEducation.Id,
                            GroupsList = context.Groups.Include(g => g.Name).Select(p => new SelectListItem { Value = p.Id.ToString(), Text = $"{p.Name.Name}-{p.Number}" }),
                            FormEducationList = context.FormEducations.Select(p => new SelectListItem { Value = p.Id.ToString(), Text = p.Name }),
                            Id = id
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
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByIdAsync(model.Id);

                if (user != null)
                {
                    var result = await repository.EditUser(user, model);

                    if (result.Succeeded)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Пользователь не найден.");
                }
            }

            return View("EditAdmin", model);
        }

        [HttpPost]
        public async Task<IActionResult> EditEmployer(EmployerEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByIdAsync(model.Id);

                if (user != null)
                {
                    if (ModelState.IsValid)
                    {
                        var result = await repository.EditUser(user, model);

                        if (result.Succeeded)
                        {
                            return RedirectToAction(nameof(Index));
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Пользователь не найден.");
                }
            }

            return View("EditEmployer", model);
        }

        [HttpPost]
        public async Task<IActionResult> EditTeacher(TeacherEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByIdAsync(model.Id);

                if (user != null)
                {
                    if (ModelState.IsValid)
                    {
                        var result = await repository.EditUser(user, model);

                        if (result.Succeeded)
                        {
                            return RedirectToAction(nameof(Index));
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Пользователь не найден.");
                }
            }

            return View("EditTeacher", model);
        }

        [HttpPost]
        public async Task<IActionResult> EditStudent(StudentEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByIdAsync(model.Id);

                if (user != null)
                {

                    var result = await repository.EditUser(user, model);

                    if (result.Succeeded)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Пользователь не найден.");
                }
            }

            return View("EditStudent", model);
        }

        [HttpPost]
        public async Task<IActionResult> AddStudent(AddStudentViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new User()
                {
                    UserName = model.Email,
                    Email = model.Email,
                    Name = model.Name,
                    Photo = UploadedImage(model)
                };

                var result = await repository.AddUser(user, model.Password,
                    new Student()
                    {
                        User = user,
                        Gender = model.Gender,
                        Dob = model.Dob,
                        FormEducation = context.FormEducations.FirstOrDefault(f => f.Id == model.FormEducation),
                        Group = context.Groups.FirstOrDefault(g => g.Id == model.Group)
                    });

                if (result.Succeeded)
                {
                    var code = await userManager.GenerateEmailConfirmationTokenAsync(user);
                    await userManager.ConfirmEmailAsync(user, code);

                    return RedirectToAction("Index");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }

            return View("AddUsers", new AddUsersViewModel(context));
        }

        [HttpPost]
        public async Task<IActionResult> AddTeacher(AddTeacherViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new User()
                {
                    UserName = model.Email,
                    Email = model.Email,
                    Name = model.Name,
                    Photo = UploadedImage(model)
                };

                var result = await repository.AddUser(user, model.Password,
                    new Teacher()
                    {
                        User = user,
                        Gender = model.Gender,
                        Department = context.Departments.FirstOrDefault(d => d.Id == model.Department)
                    });

                if (result.Succeeded)
                {
                    var code = await userManager.GenerateEmailConfirmationTokenAsync(user);
                    await userManager.ConfirmEmailAsync(user, code);

                    return RedirectToAction("Index");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }

            return View("AddUsers", new AddUsersViewModel(context));
        }

        private byte[] UploadedImage(IAddUserViewModel model)
        {
            byte[] photo = null;

            if (model.UploadPhoto != null)
            {
                if (model.UploadPhoto.Length > 0)
                {
                    using (var binaryReader = new BinaryReader(model.UploadPhoto.OpenReadStream()))
                    {
                        photo = binaryReader.ReadBytes((int)model.UploadPhoto.Length);
                    }

                    return photo;
                }
            }
            return photo;
        }

        [HttpPost]
        public async Task<IActionResult> AddDepartment(AddDepartmentsViewModel model)
        {
            if (ModelState.IsValid)
            {
                var department = new Department()
                {
                    Name = model.Name
                };

                var result = await dataRepository.AddDepartment(department);

                if (result)
                {
                    return RedirectToAction("AddData");
                }
            }

            return View("AddData", new AddDataViewModel(context));
        }
    }
}