using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using StudentAchievements.Models;

namespace StudentAchievements.Areas.Authorization.Models
{
    public class UserRepository : IUserRepository
    {
        private ApplicationDbContext applicationContext;
        private IdentityDbContext identityContext;
        private UserManager<IdentityUser> userManager;

        public UserRepository(ApplicationDbContext _applicationContext, IdentityDbContext _identityContext, UserManager<IdentityUser> _userManager)
        {
            applicationContext = _applicationContext;
            identityContext = _identityContext;
            userManager = _userManager;
        }


        public IQueryable<IdentityUser> Users => identityContext.Users;

        public async Task<IdentityResult> AddUser(IdentityUser user, string password, IUser userType)
        {
            var result = await userManager.CreateAsync(user, password);

            if (result.Succeeded)
            {
                switch (userType.GetType().Name)
                {
                    case nameof(Student):
                        await applicationContext.Students.AddAsync((Student) userType);
                        await applicationContext.SaveChangesAsync();
                        break;
                    case nameof(Employer):
                        await applicationContext.Employers.AddAsync((Employer)userType);
                        await applicationContext.SaveChangesAsync();
                        break;
                    case nameof(Teacher):
                        await applicationContext.Teachers.AddAsync((Teacher)userType);
                        await applicationContext.SaveChangesAsync();
                        break;
                }
            }

            return result;
        }

        public void EditUser(IdentityUser user)
        {
            throw new NotImplementedException();
        }

        public void DeleteUser(IdentityUser user)
        {
            throw new NotImplementedException();
        }
    }
}
