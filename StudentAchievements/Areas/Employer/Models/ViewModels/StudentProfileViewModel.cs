using StudentAchievements.Infrastructure;
using StudentAchievements.Areas.Authorization.Models;
using System.Collections.Generic;
using StudentAchievements.Models;

namespace StudentAchievements.Areas.Employer.Models.ViewModels
{
    public class StudentProfileViewModel : IStudentProfileViewModel
    {
        public StudentProfileViewModel()
        {
            NotFoundUserPhoto = NotFoundImageUtility.GetNotFoundImage();  
        }

        public byte[] NotFoundUserPhoto { get; set; }
        public StudentAchievements.Areas.Authorization.Models.Student Student { get;set; }
        public List<Assessment> AssessmentsList { get;set; }
        public List<Achievement> AchievementsList { get;set; }
        public NewMessageViewModel NewMessage { get;set; }
    }
}