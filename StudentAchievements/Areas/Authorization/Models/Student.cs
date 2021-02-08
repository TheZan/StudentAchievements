using System;
using System.Collections.Generic;

namespace StudentAchievements.Areas.Authorization.Models
{
    public class Student : IUser
    {
        public int Id { get; set; }
        public User User { get; set; }
        public DateTime Dob { get; set; }
        public Group Group { get; set; }
        public int YearOfStudy { get; set; }
        public FormEducation FormEducation { get; set; }
        public List<Assessment> Assessments { get; set; }
        public List<Achievement> Achievements { get; set; }
    }
}