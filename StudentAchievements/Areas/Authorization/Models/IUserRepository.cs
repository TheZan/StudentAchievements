using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using StudentAchievements.Areas.Admin.Models.ViewModels;
using StudentAchievements.Areas.Teacher.Models.ViewModels;
using System.Collections.Generic;
using StudentAchievements.Areas.Student.Models.ViewModels;

namespace StudentAchievements.Areas.Authorization.Models
{
    public interface IUserRepository
    {
        IQueryable<User> Users { get; }
        IQueryable<Teacher> Teachers { get; }
        IQueryable<Student> Students { get; }
        IQueryable<Employer> Employers { get; }
        IQueryable<Administrator> Administrators { get; }

        Task<IdentityResult> AddUser(User user, string password, IUser userType);
        Task<IdentityResult> EditUser(User user, IEditUserViewModel model);
        Task<IdentityResult> DeleteUser(User user);
        Task<bool> EditStudentAssessments(IList<AssessmentViewModel> model);

        Task<IdentityResult> ChangeUserPassword (User user, string oldPassword, string newPassword, string ConfirmNewPassword);
        Task<IdentityResult> EditStudent(User user, StudentSettingsViewModel model);
    }
}