using StudentAchievements.Infrastructure;
using StudentAchievements.Areas.Authorization.Models;
using System.Collections.Generic;

namespace StudentAchievements.Areas.Employer.Models.ViewModels
{
    public class StudentProfileViewModel
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