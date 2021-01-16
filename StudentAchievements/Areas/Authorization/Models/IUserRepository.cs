using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentAchievements.Areas.Authorization.Models
{
    public interface IUserRepository
    {
        IQueryable<User> Users { get; }

        void AddUser(User user);
        void EditUser(User user);
        void DeleteUser(User user);
    }
}
