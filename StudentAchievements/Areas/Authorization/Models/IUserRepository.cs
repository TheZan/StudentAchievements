using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace StudentAchievements.Areas.Authorization.Models
{
    public interface IUserRepository
    {
        IQueryable<IdentityUser> IdentityUsers { get; }
        List<IUser> ApplicationUsers { get; }

        Task<IdentityResult> AddUser(IdentityUser user, string password, IUser userType);
        void EditUser(IdentityUser user);
        Task<IdentityResult> DeleteUser(IdentityUser user);
        void AddTestData();
    }
}
