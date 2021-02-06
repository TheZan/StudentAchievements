using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using StudentAchievements.Areas.Admin.Models.ViewModels;
using StudentAchievements.Models;

namespace StudentAchievements.Areas.Authorization.Models
{
    public class UserRepository : IUserRepository
    {
        private UserManager<User> userManager;
        private RoleManager<IdentityRole> roleManager;
        private StudentAchievementsDbContext context;

        public UserRepository(UserManager<User> _userManager, RoleManager<IdentityRole> _roleManager, StudentAchievementsDbContext _context)
        {
            userManager = _userManager;
            roleManager = _roleManager;
            context = _context;
        }

        public enum Roles
        {
            Admin,
            Employer,
            Teacher,
            Student
        }

        public IQueryable<User> Users => userManager.Users;

        public async Task<IdentityResult> AddUser(User user, string password, IUser userType)
        {
            if (user != null)
            {
                var addUserResult = await userManager.CreateAsync(user, password);
                string role = String.Empty;

                if (addUserResult.Succeeded)
                {
                    switch (userType.GetType().Name)
                    {
                        case "Administrator":
                            role = Roles.Admin.ToString();
                            break;
                        case "Employer":
                            role = Roles.Employer.ToString();
                            break;
                        case "Teacher":
                            role = Roles.Teacher.ToString();
                            break;
                        case "Student":
                            role = Roles.Student.ToString();
                            break;
                    }

                    var addUserRoleResult = await userManager.AddToRoleAsync(user, role);

                    if (addUserRoleResult.Succeeded)
                    {
                        try
                        {
                            switch (role)
                            {
                                case "Admin":
                                    await context.Administrators.AddAsync((Administrator) userType);
                                    await context.SaveChangesAsync();
                                    break;
                                case "Employer":
                                    await context.Employers.AddAsync((Employer) userType);
                                    await context.SaveChangesAsync();
                                    break;
                                case "Teacher":
                                    await context.Teachers.AddAsync((Teacher) userType);
                                    await context.SaveChangesAsync();
                                    break;
                                case "Student":
                                    await context.Students.AddAsync((Student) userType);
                                    await context.SaveChangesAsync();
                                    break;
                            }

                            return IdentityResult.Success;
                        }
                        catch
                        {
                            return IdentityResult.Failed();
                        }
                    }
                }
            }

            return IdentityResult.Failed();
        }

        public async Task<IdentityResult> EditUser(User user, IEditViewModel model)
        {
            if (user != null)
            {
                user.Email = model.Email;
                user.UserName = model.Email;
                user.Name = model.Name;
                user.Photo = UploadedImage(model) ?? model.Photo;
                
                var userUpdateResult = await userManager.UpdateAsync(user);
                if (userUpdateResult.Succeeded)
                {
                    var role = await userManager.GetRolesAsync(user);

                    switch (role.First())
                    {
                        case "Employer":
                            var employer = context.Employers.FirstOrDefault(u => u.User.Email == user.Email);
                            employer.Description = ((EmployerEditViewModel) model).Description;
                            await context.SaveChangesAsync();
                            break;
                        case "Teacher":
                            var teacher = context.Teachers.FirstOrDefault(u => u.User.Email == user.Email);
                            teacher.Department = ((TeacherEditViewModel) model).Department;
                            await context.SaveChangesAsync();
                            break;
                        case "Student":
                            var student = context.Students.FirstOrDefault(u => u.User.Email == user.Email);
                            student.Dob = ((StudentEditViewModel) model).Dob;
                            student.Group = ((StudentEditViewModel) model).Group;
                            await context.SaveChangesAsync();
                            break;
                    }

                    return IdentityResult.Success;
                }
            }

            return IdentityResult.Failed();
        }

        public async Task<IdentityResult> DeleteUser(User user)
        {
            if (user != null)
            {
                var role = await userManager.GetRolesAsync(user);

                switch (role.First())
                {
                    case "Admin":
                        var admin = context.Administrators.FirstOrDefault(u => u.User.Email == user.Email);
                        context.Administrators.Remove(admin);
                        await context.SaveChangesAsync();
                        break;
                    case "Employer":
                        var employer = context.Employers.FirstOrDefault(u => u.User.Email == user.Email);
                        context.Employers.Remove(employer);
                        await context.SaveChangesAsync();
                        break;
                    case "Teacher":
                        var teacher = context.Teachers.FirstOrDefault(u => u.User.Email == user.Email);
                        context.Teachers.Remove(teacher);
                        await context.SaveChangesAsync();
                        break;
                    case "Student":
                        var student = context.Students.FirstOrDefault(u => u.User.Email == user.Email);
                        context.Students.Remove(student);
                        await context.SaveChangesAsync();
                        break;
                }

                var deleteResult = await userManager.DeleteAsync(user);

                if (deleteResult.Succeeded)
                {
                    return IdentityResult.Success;
                }
            }

            return IdentityResult.Failed();
        }

        private byte[] UploadedImage(IEditViewModel model)
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