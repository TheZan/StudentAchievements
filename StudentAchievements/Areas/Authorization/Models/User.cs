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
        public byte[] Photo { get; set; }
    }
}
