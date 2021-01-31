using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentAchievements.Areas.Admin.Models.ViewModels;
using StudentAchievements.Areas.Authorization.Models;

namespace StudentAchievements.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private IUserRepository repository;

        public AdminController(IUserRepository _repository)
        {
            repository = _repository;
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
            var usersModel = new UsersListViewModel()
            {
                ApplicationUsers = repository.ApplicationUsers,
                IdentityUsers = repository.IdentityUsers
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

            double pageCount = (double)(repository.IdentityUsers.Count() / Convert.ToDecimal(maxRows));
            usersModel.PageCount = (int)Math.Ceiling(pageCount);

            usersModel.CurrentPageIndex = currentPage;

            return usersModel;
        }
    }
}
