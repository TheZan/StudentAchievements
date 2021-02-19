using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentAchievements.Areas.Authorization.Models
{
    public class Score
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Assessment> Assessments { get; set; }
    }
}
