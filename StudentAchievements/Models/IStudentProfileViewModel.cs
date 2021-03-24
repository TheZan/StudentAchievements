using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentAchievements.Models
{
    public interface IStudentProfileViewModel
    {
        public byte[] NotFoundUserPhoto { get; set; }
        public StudentAchievements.Areas.Authorization.Models.Student Student { get;set; }
    }
}