using System.Reflection.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentAchievements.Areas.Authorization.Models;
using StudentAchievements.Areas.Authorization.Models.ViewModels;

namespace StudentAchievements.Areas.Authorization.Components
{
    public class ChangePasswordViewComponent : ViewComponent
    {
        private IUserRepository userRepository;

        public ChangePasswordViewComponent(IUserRepository _userRepository)
        {
            userRepository = _userRepository;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var currentUser = await userRepository.Users.FirstOrDefaultAsync(p => p.Email == User.Identity.Name);
            return View(new ChangePasswordViewModel() { Id = currentUser.Id });
        }
    }
}