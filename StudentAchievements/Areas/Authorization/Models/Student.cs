using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentAchievements.Areas.Authorization.Models
{
    public class Student : IUser
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public byte[] Photo { get; set; }
        public string Email { get; set; }
        public DateTime Dob { get; set; }
        public Group Group { get; set; }
        public IQueryable<Assessment> Assessments { get; set; }
        public IQueryable<Achievement> Achievements { get; set; }
    }
}