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

            GetApplicationUsers();
        }


        public IQueryable<IdentityUser> IdentityUsers => identityContext.Users;
        public List<IUser> ApplicationUsers { get; set; }

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
                    case nameof(Administrator):
                        await applicationContext.Administrators.AddAsync((Administrator)userType);
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

        public async Task<IdentityResult> DeleteUser(IdentityUser user)
        {
            var role = await userManager.GetRolesAsync(user);
            IUser appUser;

            switch (role.First())
            {
                case "Admin":
                    appUser = applicationContext.Administrators.FirstOrDefault(u => u.Email == user.Email);
                    applicationContext.Administrators.Remove((Administrator) appUser);
                    await applicationContext.SaveChangesAsync();
                    break;
                case "Employer":
                    appUser = applicationContext.Employers.FirstOrDefault(u => u.Email == user.Email);
                    applicationContext.Employers.Remove((Employer) appUser);
                    await applicationContext.SaveChangesAsync();
                    break;
                case "Teacher":
                    appUser = applicationContext.Teachers.FirstOrDefault(u => u.Email == user.Email);
                    applicationContext.Teachers.Remove((Teacher) appUser);
                    await applicationContext.SaveChangesAsync();
                    break;
                case "Student":
                    appUser = applicationContext.Students.FirstOrDefault(u => u.Email == user.Email);
                    applicationContext.Students.Remove((Student) appUser);
                    await applicationContext.SaveChangesAsync();
                    break;
            }

            return await userManager.DeleteAsync(user);
        }

        private void GetApplicationUsers()
        {
            ApplicationUsers = new List<IUser>();
            ApplicationUsers.AddRange(applicationContext.Employers);
            ApplicationUsers.AddRange(applicationContext.Students);
            ApplicationUsers.AddRange(applicationContext.Teachers);
            ApplicationUsers.AddRange(applicationContext.Administrators);
        }

        public void AddTestData()
        {
            var userList = new List<IUser>()
            {
                new Employer()
                {
                    Email = "1@mail.ru",
                    Description = "sada",
                    Name = "rftge"
                },
                new Student()
                {
                    Name = "sdasdijrtoyo"
                },
                new Teacher()
                {
                    Email = "sadas",
                    Name = "asdasdrieio"
                },
                new Student()
                {
                    Name = "sdasadsdijrtoyo"
                },
                new Student()
                {
                    Name = "sdasdi2j1rtoyo"
                },
                new Student()
                {
                    Name = "sdasdijdsf34rtoyo"
                },
                new Student()
                {
                    Name = "sdasdiasd12jrtoyo"
                },
                new Student()
                {
                    Name = "sdasdij234rtoyo"
                },
                new Student()
                {
                    Name = "sdasdij234rtoyo"
                },
                new Student()
                {
                    Name = "sdasd2311ijrtoyo"
                },
                new Student()
                {
                    Name = "sdasdijrdfgtoyo"
                },
                new Student()
                {
                    Name = "sdasdijklkrtoyo"
                },
                new Student()
                {
                    Name = "sadas"
                },
                new Student()
                {
                    Name = "loiui112"
                },
                new Student()
                {
                    Name = "fdgh232"
                },
                new Student()
                {
                    Name = "fdgdfg34"
                },
                new Student()
                {
                    Name = "erdf2"
                },
                new Student()
                {
                    Name = "fgh34"
                },
                new Student()
                {
                    Name = "iu21"
                },
                new Student()
                {
                    Name = "354"
                },
                new Student()
                {
                    Name = "345"
                },
                new Student()
                {
                    Name = "hfg"
                },
                new Student()
                {
                    Name = "234"
                },
                new Student()
                {
                    Name = "dfs"
                },
                new Student()
                {
                    Name = "zxc"
                },
                new Student()
                {
                    Name = "678"
                },
                new Student()
                {
                    Name = "zxcfv"
                },
                new Student()
                {
                    Name = "234"
                },
                new Student()
                {
                    Name = "qwe"
                },
                new Student()
                {
                    Name = "dsf"
                },
                new Student()
                {
                    Name = "32"
                },
                new Student()
                {
                    Name = "543"
                },
                new Student()
                {
                    Name = "dfg"
                },
                new Student()
                {
                    Name = "dfg"
                },
                new Student()
                {
                    Name = "123"
                },
                new Student()
                {
                    Name = "23"
                },
                new Student()
                {
                    Name = "6"
                },
                new Student()
                {
                    Name = "5"
                },
                new Student()
                {
                    Name = "4"
                },
                new Student()
                {
                    Name = "123"
                },
                new Student()
                {
                    Name = "asd"
                },
            };

            foreach (var user in userList)
            {
                switch (user.GetType().Name)
                {
                    case "Employer":
                        applicationContext.Employers.Add((Employer) user);
                        break;
                    case "Teacher":
                        applicationContext.Teachers.Add((Teacher)user);
                        break;
                    case "Student":
                        applicationContext.Students.Add((Student)user);
                        break;
                }
            }

            applicationContext.SaveChanges();
        }
    }
}
