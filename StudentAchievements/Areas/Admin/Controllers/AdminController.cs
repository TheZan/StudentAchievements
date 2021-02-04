using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StudentAchievements.Areas.Admin.Models.ViewModels;
using StudentAchievements.Areas.Authorization.Models;

namespace StudentAchievements.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private UserManager<User> userManager;
        private IUserRepository repository;

        public AdminController(IUserRepository _repository, UserManager<User> _userManager)
        {
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
                //var model = new EditUserViewModel()
                //{
                //    UserName = applicationUser.Name,
                //    Email = applicationUser.Email
                //};

                return View();
            }
            else
            {
                ModelState.AddModelError("", "Пользователь не найден.");
                return StatusCode(StatusCodes.Status404NotFound);
            }
        }

        [HttpPost]
        public async Task<IActionResult> EditUser(IEditViewModel model)
        {
            var user = await userManager.FindByEmailAsync(model.Email);

            if (user != null)
            {
                var result = await repository.EditUser(user, model);

                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(Index));
                }

                return View(model);
            }
            else
            {
                ModelState.AddModelError("", "Пользователь не найден.");
                return StatusCode(StatusCodes.Status404NotFound);
            }
        }
    }
}