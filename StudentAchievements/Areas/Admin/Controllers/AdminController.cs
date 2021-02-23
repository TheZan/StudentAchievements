using System.Security.Permissions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DynamicVML;
using DynamicVML.Extensions;
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

        public async Task<IActionResult> Index(string searchString, int? pageNumber)
        {
            ViewBag.UsersSelected = "active";
            ViewBag.AddUsersSelected = "";
            ViewBag.DataSelected = "";

            if (searchString != null)
            {
                pageNumber = 1;
            }

            ViewData["CurrentFilter"] = searchString;

            var user = repository.Users;
            if (!String.IsNullOrEmpty(searchString))
            {
                user = user.Where(s => s.Name.Contains(searchString)
                                       || s.Email.Contains(searchString));
            }

            int pageSize = 10;

            return View(new UsersListViewModel(userManager)
            {
                Users = await PaginatedList<User>.CreateAsync(user.AsNoTracking(), pageNumber ?? 1, pageSize)
            });
        }

        public ViewResult AddUsers()
        {
            ViewBag.UsersSelected = "";
            ViewBag.AddUsersSelected = "active";
            ViewBag.DataSelected = "";

            return View(new AddUsersViewModel(context));
        }

        public ViewResult AddData()
        {
            ViewBag.UsersSelected = "";
            ViewBag.AddUsersSelected = "";
            ViewBag.DataSelected = "active";

            return View(new AddDataViewModel(context));
        }

        public IActionResult AddStudents() => PartialView("AddStudents");

        public IActionResult AddTeachers() => PartialView("AddTeachers");

        public IActionResult AddDepartments() => PartialView("AddDepartments");

        public IActionResult AddDirections() => PartialView("AddDirections");

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
                            GroupsList = context.Groups.Include(g => g.Direction).Select(p => new SelectListItem { Value = p.Id.ToString(), Text = $"{p.Direction.GroupName}-{p.Grade}{p.Number}" }),
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

        #region Admin

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

        #endregion

        #region Employer

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

        #endregion

        #region Teacher

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
                    new Authorization.Models.Teacher()
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

        #endregion

        #region Student

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
                
                var group = context.Groups.FirstOrDefault(g => g.Id == model.Group);

                var result = await repository.AddUser(user, model.Password,
                    new Student()
                    {
                        User = user,
                        Gender = model.Gender,
                        Dob = model.Dob,
                        FormEducation = context.FormEducations.FirstOrDefault(f => f.Id == model.FormEducation),
                        Group = group
                    });

                var subjects = dataRepository.Directions.Include(s => s.Subjects).FirstOrDefault(d => d.Id == group.DirectionId);
                var assesments = new List<Assessment>();
                var student = context.Students.FirstOrDefault(s => s.User.Email == model.Email);
                var noScore = context.Scores.FirstOrDefault(s => s.Name == "Нет оценки");

                foreach (var subject in subjects.Subjects)
                {
                    assesments.Add(new Assessment()
                    {
                        Subject = subject,
                        Student = student,
                        Score = noScore
                    });
                }

                context.Assessments.AddRange(assesments);
                context.SaveChanges();

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

        #endregion

        #region Department

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

        [HttpPost]
        public async Task<IActionResult> DeleteDepartment(int id)
        {
            var department = context.Departments.FirstOrDefault(d => d.Id == id);

            if (department != null)
            {
                await dataRepository.DeleteDepartment(department);
            }

            return RedirectToAction("AddData");
        }

        [HttpGet]
        public async Task<IActionResult> EditDepartment(int id)
        {
            var department = await context.Departments.FirstOrDefaultAsync(d => d.Id == id);

            if (department != null)
            {
                var model = new EditDepartmentViewModel()
                {
                    Id = department.Id,
                    Name = department.Name
                };

                return View("EditDepartment", model);
            }
            else
            {
                ModelState.AddModelError("", "Факультет не найден.");
            }

            return StatusCode(StatusCodes.Status404NotFound);
        }

        [HttpPost]
        public async Task<IActionResult> EditDepartment(EditDepartmentViewModel model)
        {
            if (ModelState.IsValid)
            {
                var department = await context.Departments.FirstOrDefaultAsync(d => d.Id == model.Id);

                if (department != null)
                {

                    department.Name = model.Name;

                    if (await dataRepository.EditDepartment(department))
                    {
                        return RedirectToAction(nameof(AddData));
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Факультет не найден.");
                }
            }

            return View("EditDepartment", model);
        }

        #endregion

        #region Direction

        [HttpPost]
        public async Task<IActionResult> AddDirection(AddDirectionsViewModel model)
        {
            if (ModelState.IsValid)
            {
                var direction = new Direction()
                {
                    Name = model.Name,
                    GroupName = model.GroupName,
                    Department = await context.Departments.FirstOrDefaultAsync(d => d.Id == model.Department),
                    ProgramType = await context.ProgramType.FirstOrDefaultAsync(d => d.Id == model.ProgramType)
                };

                var result = await dataRepository.AddDirection(direction);

                if (result)
                {
                    var subjectsViewModels = model.SubjectsList.ToModel(s => new AddSubjectViewModel()
                    {
                        Name = s.Name,
                        Grade = s.Grade,
                        Semester = s.Semester,
                        ControlType = s.ControlType
                    }).ToList();

                    var subjects = new List<Subject>();

                    foreach (var viewModel in subjectsViewModels)
                    {
                        subjects.Add(new Subject()
                        {
                            Name = viewModel.Name,
                            Grade = viewModel.Grade,
                            Semester = viewModel.Semester,
                            Direction = direction,
                            ControlType = await context.ControlTypes.FirstOrDefaultAsync(c => c.Id == viewModel.ControlType)
                        });
                    }

                    foreach (var subject in subjects)
                    {
                        await dataRepository.AddSubject(subject);
                    }

                    return RedirectToAction("AddData");
                }
            }

            return View("AddData", new AddDataViewModel(context));
        }

        [HttpPost]
        public async Task<IActionResult> DeleteDirection(int id)
        {
            var direction = context.Directions.FirstOrDefault(d => d.Id == id);

            if (direction != null)
            {
                await dataRepository.DeleteDirection(direction);
            }

            return RedirectToAction("AddData");
        }

        [HttpGet]
        public async Task<IActionResult> EditDirection(int id)
        {
            var direction = await context.Directions.Include(d => d.Department).Include(p => p.ProgramType).Include(s => s.Subjects).ThenInclude(t => t.ControlType)
                .FirstOrDefaultAsync(d => d.Id == id);

            if (direction != null)
            {
                var model = new EditDirectionViewModel()
                {
                    Id = direction.Id,
                    Name = direction.Name,
                    GroupName = direction.GroupName,
                    Department = direction.Department.Id,
                    ProgramType = direction.ProgramType.Id,
                    DepartmentsList = context.Departments.Select(p => new SelectListItem { Value = p.Id.ToString(), Text = p.Name }),
                    ProgramTypeList = context.ProgramType.Select(p => new SelectListItem { Value = p.Id.ToString(), Text = p.Name }),
                    Subjects = direction.Subjects
                };

                return View("EditDirection", model);
            }
            else
            {
                ModelState.AddModelError("", "Направление не найдено.");
            }

            return StatusCode(StatusCodes.Status404NotFound);
        }

        [HttpPost]
        public async Task<IActionResult> EditDirection(EditDirectionViewModel model)
        {
            if (ModelState.IsValid)
            {
                var direction = await context.Directions.Include(d => d.Department).Include(p => p.ProgramType)
                .FirstOrDefaultAsync(d => d.Id == model.Id);

                if (direction != null)
                {
                    direction.Name = model.Name;
                    direction.GroupName = model.GroupName;
                    direction.Department = await context.Departments.FirstOrDefaultAsync(d => d.Id == model.Department);
                    direction.ProgramType = await context.ProgramType.FirstOrDefaultAsync(d => d.Id == model.ProgramType);

                    if (await dataRepository.EditDirection(direction))
                    {
                        var subjectsViewModels = model.SubjectsList.ToModel(s => new AddSubjectViewModel()
                        {
                            Name = s.Name,
                            Grade = s.Grade,
                            Semester = s.Semester,
                            ControlType = s.ControlType
                        }).ToList();

                        var subjects = new List<Subject>();

                        foreach (var viewModel in subjectsViewModels)
                        {
                            subjects.Add(new Subject()
                            {
                                Name = viewModel.Name,
                                Grade = viewModel.Grade,
                                Semester = viewModel.Semester,
                                Direction = direction,
                                ControlType = await context.ControlTypes.FirstOrDefaultAsync(c => c.Id == viewModel.ControlType)
                            });
                        }

                        foreach (var subject in subjects)
                        {
                            subject.Direction = direction;
                            await dataRepository.AddSubject(subject);
                        }

                        return RedirectToAction(nameof(AddData));
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Направление не найдено.");
                }
            }

            return View("EditDirection", model);
        }

        #endregion

        #region Group

        [HttpPost]
        public async Task<IActionResult> AddGroup(AddGroupsViewModel model)
        {
            if (ModelState.IsValid)
            {
                var group = new Group()
                {
                    Grade = model.Grade,
                    Number = model.Number,
                    Direction = await context.Directions.FirstOrDefaultAsync(d => d.Id == model.Direction),
                };

                var result = await dataRepository.AddGroup(group);

                if (result)
                {
                    return RedirectToAction("AddData");
                }
            }

            return View("AddData", new AddDataViewModel(context));
        }

        [HttpPost]
        public async Task<IActionResult> DeleteGroup(int id)
        {
            var group = context.Groups.FirstOrDefault(d => d.Id == id);

            if (group != null)
            {
                await dataRepository.DeleteGroup(group);
            }

            return RedirectToAction("AddData");
        }

        [HttpGet]
        public async Task<IActionResult> EditGroup(int id)
        {
            var group = await context.Groups.Include(d => d.Direction).FirstOrDefaultAsync(d => d.Id == id);

            if (group != null)
            {
                var model = new EditGroupViewModel()
                {
                    Id = group.Id,
                    Direction = group.Direction.Id,
                    Number = group.Number,
                    Grade = group.Grade,
                    DirectionsList = context.Directions.Select(p => new SelectListItem { Value = p.Id.ToString(), Text = p.Name }),
                };

                return View("EditGroup", model);
            }
            else
            {
                ModelState.AddModelError("", "Группа не найдена.");
            }

            return StatusCode(StatusCodes.Status404NotFound);
        }

        [HttpPost]
        public async Task<IActionResult> EditGroup(EditGroupViewModel model)
        {
            if (ModelState.IsValid)
            {
                var group = await context.Groups.Include(d => d.Direction).FirstOrDefaultAsync(d => d.Id == model.Id);

                if (group != null)
                {

                    group.Number = model.Number;
                    group.Grade = model.Grade;
                    group.Direction = await context.Directions.FirstOrDefaultAsync(d => d.Id == model.Direction);

                    if (await dataRepository.EditGroup(group))
                    {
                        return RedirectToAction(nameof(AddData));
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Группа не найдена.");
                }
            }

            return View("EditGroup", model);
        }

        #endregion

        #region Subject

        public IActionResult AddSubject(AddNewDynamicItem parameters)
        {
            var typeList = context.ControlTypes.Select(p => new SelectListItem
                {Value = p.Id.ToString(), Text = p.Name});

            return this.PartialView(new AddSubjectViewModel()
            {
                ControlTypeList = typeList
            }, parameters);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteSubject(int id)
        {
            var subject = await context.Subjects.FirstOrDefaultAsync(s => s.Id == id);

            if(subject != null)
            {
                await dataRepository.DeleteSubject(subject);
            }

            return RedirectToAction("EditDirection", new { id = subject.DirectionId });
        }

        #endregion

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
    }
}