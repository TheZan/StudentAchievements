using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StudentAchievements.Models;
using StudentAchievements.Infrastructure;
using StudentAchievements.Areas.Authorization.Models;
using StudentAchievements.Areas.Teacher.Models.ViewModels;

namespace StudentAchievements.Areas.Student.Models.ViewModels
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
        public AchievementViewModel AchievementViewModel { get;set; }
    }
}