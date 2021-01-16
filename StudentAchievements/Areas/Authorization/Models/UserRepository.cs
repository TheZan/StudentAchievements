using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StudentAchievements.Models;

namespace StudentAchievements.Areas.Authorization.Models
{
    public class UserRepository : IUserRepository
    {
        private ApplicationDbContext context;

        public UserRepository(ApplicationDbContext _context)
        {
            context = _context;
        }


        public IQueryable<User> Users => context.Users;

        public void AddUser(User user)
        {
            throw new NotImplementedException();
        }

        public void EditUser(User user)
        {
            throw new NotImplementedException();
        }

        public void DeleteUser(User user)
        {
            throw new NotImplementedException();
        }
    }
}
