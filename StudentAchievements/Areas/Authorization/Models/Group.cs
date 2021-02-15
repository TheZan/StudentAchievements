using System.Collections.Generic;

namespace StudentAchievements.Areas.Authorization.Models
{
    public class Group
    {
        public int Id { get; set; }
        public int Number { get; set; }
        public int DirectionId { get; set; }
        public Direction Direction { get; set; }
        public List<Student> Students { get; set; }
    }
}