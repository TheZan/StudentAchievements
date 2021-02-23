using StudentAchievements.Infrastructure;
using StudentAchievements.Areas.Authorization.Models;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace StudentAchievements.Areas.Teacher.Models.ViewModels
{
    public class StudentProfileViewModel
    {
        public StudentProfileViewModel()
        {
            NotFoundUserPhoto = NotFoundImageUtility.GetNotFoundImage();  
        }

        public byte[] NotFoundUserPhoto { get; set; }
        public Student Student { get;set; }
        public IList<AssessmentViewModel> AssessmentsList { get;set; }
        public AchievementViewModel AchievementViewModel { get;set; }
    }
}