using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StudentAchievements.Areas.Vacancies.Models;

namespace StudentAchievements.Areas.Authorization.Models
{
    public class Employer : IUser
    {
        public int Id { get; set; }
        public User User { get; set; }
        public string Description { get; set; }
        public List<Vacancy> Vacancies { get;set; }
    }
}
