using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StudentAchievements.Areas.Authorization.Models;
using StudentAchievements.Infrastructure;

namespace StudentAchievements.Areas.Teacher.Models.ViewModels
{
    public class StudentListViewModel
    {
        public StudentListViewModel()
        {
            NotFoundUserPhoto = NotFoundImageUtility.GetNotFoundImage();
        }

        public byte[] NotFoundUserPhoto { get; set; }

        public IEnumerable<Direction> Directions { get; set; }
    }
}