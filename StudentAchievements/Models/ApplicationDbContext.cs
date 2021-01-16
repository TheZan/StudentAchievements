using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using StudentAchievements.Areas.Authorization.Models;

namespace StudentAchievements.Models
{
    public sealed class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Assessments> Assessments { get; set; }
        public DbSet<Achievements> Achievements { get; set; }
        public DbSet<StudentGroup> StudentGroups { get; set; }
    }
}
