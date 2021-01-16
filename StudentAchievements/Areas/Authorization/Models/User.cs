using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace StudentAchievements.Areas.Authorization.Models
{
    public class User : IdentityUser
    {
        public string Name { get; set; }
        public DateTime Dob { get; set; }
        public StudentGroup Group { get; set; }
        public IQueryable<Achievements> Achievements { get; set; }
        public IQueryable<Assessments> Assessments { get; set; }
    }
}
