﻿namespace StudentAchievements.Areas.Authorization.Models
{
    public class Achievement
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Student Student { get; set; }
    }
}