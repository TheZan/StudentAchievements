using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StudentAchievements.Infrastructure;
using StudentAchievements.Areas.Authorization.Models;
using StudentAchievements.Areas.Employer.Models;

namespace StudentAchievements.Areas.Employer.Models.ViewModels
{
    public class StudentListViewModel
    {
        public StudentListViewModel()
        {
            NotFoundUserPhoto = NotFoundImageUtility.GetNotFoundImage();
        }

        public PaginatedList<StudentAchievements.Areas.Authorization.Models.Student> Students { get; set; }

        public byte[] NotFoundUserPhoto { get; set; }

        public ScoreCountModel GetScoreCount(StudentAchievements.Areas.Authorization.Models.Student student) => new ScoreCountModel()
        {
            Offset = student.Assessments.Where(p => p.Score.Name == "Зачет").Count(),
            Five = student.Assessments.Where(p => p.Score.Name == "Отлично").Count(),
            Four = student.Assessments.Where(p => p.Score.Name == "Хорошо").Count(),
            Three = student.Assessments.Where(p => p.Score.Name == "Удовлетворительно").Count()
        };
    }
}