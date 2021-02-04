﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentAchievements.Areas.Authorization.Models
{
    public class Teacher : IUser
    {
        public int Id { get; set; }
        public User User { get; set; }
        public Department Department { get; set; }
    }
}
