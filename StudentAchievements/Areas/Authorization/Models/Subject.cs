using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace StudentAchievements.Areas.Authorization.Models
{
    public class Subject
    {
        public int Id { get; set; }
        [DisplayName("Название")]
        public string Name { get; set; }
        public int Grade { get; set; }
        public int Semester { get; set; }
        public int DirectionId { get; set; }
        public Direction Direction { get; set; }
        public int ControlTypeId { get; set; }
        public ControlType ControlType { get; set; }
    }
}
