using System.Collections.Generic;

namespace StudentAchievements.Areas.Authorization.Models
{
    public class Direction
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string GroupName { get; set; }
        public int DepartmentId { get; set; }
        public Department Department { get; set; }
        public ProgramType ProgramType { get; set; }
        public List<Group> Groups { get; set; }
        public List<Subject> Subjects { get; set; }
    }
}