using System.Collections.Generic;

namespace StudentAchievements.Areas.Authorization.Models
{
    public class FormEducation
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Student> Students { get; set; }
    }
}