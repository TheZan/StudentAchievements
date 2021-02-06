using System.Collections.Generic;

namespace StudentAchievements.Areas.Authorization.Models
{
    public class ProgramType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Direction> Directions { get; set; }
    }
}