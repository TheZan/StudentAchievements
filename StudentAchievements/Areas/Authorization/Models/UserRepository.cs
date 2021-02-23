using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using StudentAchievements.Areas.Admin.Models.ViewModels;
using StudentAchievements.Areas.Teacher.Models.ViewModels;
using StudentAchievements.Models;
using Microsoft.EntityFrameworkCore;

namespace StudentAchievements.Areas.Authorization.Models
{
    public class UserRepository : IUserRepository
    {
        private UserManager<User> userManager;
        private StudentAchievementsDbContext context;

        public UserRepository(UserManager<User> _userManager, StudentAchievementsDbContext _context)
        {
            userManager = _userManager;
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
        public IQueryable<Teacher> Teachers => context.Teachers;
        public IQueryable<Student> Students => context.Students;
        public IQueryable<Employer> Employers => context.Employers;
        public IQueryable<Administrator> Administrators => context.Administrators;

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

        public async Task<IdentityResult> EditUser(User user, IEditUserViewModel model)
        {
            if (user != null)
            {
                user.UserName = model.Email;
                user.Name = model.Name;
                user.Photo = UploadedImage(model) ?? user.Photo;
                
                var userUpdateResult = await userManager.UpdateAsync(user);
                if (userUpdateResult.Succeeded)
                {
                    var role = await userManager.GetRolesAsync(user);

                    switch (role.First())
                    {
                        case "Admin":
                            var admin = context.Administrators.FirstOrDefault(u => u.User.Email == user.Email);
                            admin.Gender = ((AdminEditViewModel)model).Gender;
                            await context.SaveChangesAsync();
                            break;
                        case "Employer":
                            var employer = context.Employers.FirstOrDefault(u => u.User.Email == user.Email);
                            employer.Description = ((EmployerEditViewModel) model).Description;
                            await context.SaveChangesAsync();
                            break;
                        case "Teacher":
                            var teacher = context.Teachers.FirstOrDefault(u => u.User.Email == user.Email);
                            teacher.Department = context.Departments.FirstOrDefault(d => d.Id == ((TeacherEditViewModel)model).Department);
                            teacher.Gender = ((TeacherEditViewModel)model).Gender;
                            await context.SaveChangesAsync();
                            break;
                        case "Student":
                            var student = context.Students.FirstOrDefault(u => u.User.Email == user.Email);
                            student.Dob = ((StudentEditViewModel) model).Dob;
                            student.Group = context.Groups.FirstOrDefault(s => s.Id == ((StudentEditViewModel)model).Group);
                            student.FormEducation = context.FormEducations.FirstOrDefault(f => f.Id == ((StudentEditViewModel)model).FormEducation);
                            student.Gender = ((StudentEditViewModel)model).Gender;
                            await context.SaveChangesAsync();
                            break;
                    }

                    var token = await userManager.GenerateChangeEmailTokenAsync(user, model.Email);
                    await userManager.ChangeEmailAsync(user, model.Email, token);

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

        private byte[] UploadedImage(IEditUserViewModel model)
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

        public async Task<bool> EditStudentAssessments(IList<AssessmentViewModel> model)
        {
            if(model != null)
            {
                var student = await context.Students.Include(a => a.Assessments.OrderBy(o => o.Subject.Semester)).ThenInclude(s => s.Subject).FirstOrDefaultAsync(s => s.Id == model[0].StudentId);

                for (int i = 0; i < student.Assessments.Count; i++)
                {
                    for (int j = 0; j < model.Count; j++)
                    {
                        student.Assessments[i++].Score = await context.Scores.FirstOrDefaultAsync(s => s.Id == model[j].Score);
                    }
                }

                await context.SaveChangesAsync();

                return true;
            }

            return false;
        }
    }
}