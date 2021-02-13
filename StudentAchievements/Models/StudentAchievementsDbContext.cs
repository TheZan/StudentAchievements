using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StudentAchievements.Areas.Authorization.Models;

namespace StudentAchievements.Models
{
    public sealed class StudentAchievementsDbContext : IdentityDbContext<User>
    {
        public StudentAchievementsDbContext(DbContextOptions<StudentAchievementsDbContext> options)
            : base(options)
        {
            //Database.EnsureCreated();
        }

        public DbSet<Employer> Employers { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Administrator> Administrators { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Assessment> Assessments { get; set; }
        public DbSet<Achievement> Achievements { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<ProgramType> ProgramType { get; set; }
        public DbSet<Direction> Directions { get; set; }
        public DbSet<FormEducation> FormEducations { get; set; }
    }
}
