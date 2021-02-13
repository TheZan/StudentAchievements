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
        public Direction Direction { get; set; }
    }
}
