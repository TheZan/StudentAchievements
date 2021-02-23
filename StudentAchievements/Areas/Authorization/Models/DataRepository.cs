﻿using StudentAchievements.Models;
using System.Linq;
using System.Threading.Tasks;

namespace StudentAchievements.Areas.Authorization.Models
{
    public class DataRepository : IDataRepository
    {
        private StudentAchievementsDbContext context;

        public DataRepository(StudentAchievementsDbContext _context) => context = _context;

        public IQueryable<Department> Departments => context.Departments;
        public IQueryable<Direction> Directions => context.Directions;
        public IQueryable<Group> Groups => context.Groups;
        public IQueryable<Subject> Subjects => context.Subjects;
        public IQueryable<Score> Scores => context.Scores;
        public IQueryable<Achievement> Achievements => context.Achievements;

        public async Task<bool> AddAchievement(Achievement achievement)
        {
            if(achievement != null)
            {
                await context.Achievements.AddAsync(achievement);
                await context.SaveChangesAsync();

                return true;
            }

            return false;
        }

        public async Task<bool> AddDepartment(Department department)
        {
            if(department != null)
            {
                await context.Departments.AddAsync(department);
                await context.SaveChangesAsync();

                return true;
            }

            return false;
        }

        public async Task<bool> AddDirection(Direction direction)
        {
            if(direction != null)
            {
                await context.Directions.AddAsync(direction);
                await context.SaveChangesAsync();

                return true;
            }

            return false;
        }

        public async Task<bool> AddGroup(Group group)
        {
            if(group != null)
            {
                await context.Groups.AddAsync(group);
                await context.SaveChangesAsync();

                return true;
            }

            return false;
        }

        public async Task<bool> AddScore(Score score)
        {
             if(score != null)
             {
                await context.Scores.AddAsync(score);
                await context.SaveChangesAsync();

                return true;
             }

             return false;
        }

        public async Task<bool> AddSubject(Subject subject)
        {
            if(subject != null)
            {
                await context.Subjects.AddAsync(subject);
                await context.SaveChangesAsync();

                return true;
            }

            return false;
        }

        public async Task<bool> DeleteAchievement(Achievement achievement)
        {
            if (achievement != null)
            {
                context.Achievements.Remove(achievement);
                await context.SaveChangesAsync();

                return true;
            }

            return false;
        }

        public async Task<bool> DeleteDepartment(Department department)
        {
            if (department != null)
            {
                context.Departments.Remove(department);
                await context.SaveChangesAsync();

                return true;
            }

            return false;
        }

        public async Task<bool> DeleteDirection(Direction direction)
        {
            if (direction != null)
            {
                context.Directions.Remove(direction);
                await context.SaveChangesAsync();

                return true;
            }

            return false;
        }

        public async Task<bool> DeleteGroup(Group group)
        {
            if (group != null)
            {
                context.Groups.Remove(group);
                await context.SaveChangesAsync();

                return true;
            }

            return false;
        }

        public async Task<bool> DeleteScore(Score score)
        {
            if (score != null)
            {
                context.Scores.Remove(score);
                await context.SaveChangesAsync();

                return true;
            }

            return false;
        }

        public async Task<bool> DeleteSubject(Subject subject)
        {
            if (subject != null)
            {
                context.Subjects.Remove(subject);
                await context.SaveChangesAsync();

                return true;
            }

            return false;
        }

        public async Task<bool> EditAchievement(Achievement achievement)
        {
            if (achievement != null)
            {
                var oldAchievement = context.Achievements.FirstOrDefault(d => d.Id == achievement.Id);
                oldAchievement.Name = achievement.Name;
                oldAchievement.Description = achievement.Description;
                oldAchievement.Student = achievement.Student;

                await context.SaveChangesAsync();

                return true;
            }

            return false;
        }

        public async Task<bool> EditDepartment(Department department)
        {
            if (department != null)
            {
                var oldDepartment = context.Departments.FirstOrDefault(d => d.Id == department.Id);
                oldDepartment.Name = department.Name;
                oldDepartment.Directions = department.Directions;

                await context.SaveChangesAsync();

                return true;
            }

            return false;
        }

        public async Task<bool> EditDirection(Direction direction)
        {
            if (direction != null)
            {
                var oldDirection = context.Directions.FirstOrDefault(p => p.Id == direction.Id);
                oldDirection.Name = direction.Name;
                oldDirection.ProgramType = direction.ProgramType;
                oldDirection.Department = direction.Department;

                await context.SaveChangesAsync();

                return true;
            }

            return false;
        }

        public async Task<bool> EditGroup(Group group)
        {
            if (group != null)
            {
                var oldGroup = context.Groups.FirstOrDefault(p => p.Id == group.Id);
                oldGroup.Number = group.Number;
                oldGroup.Direction = group.Direction;

                await context.SaveChangesAsync();

                return true;
            }

            return false;
        }

        public async Task<bool> EditScore(Score score)
        {
            if (score != null)
            {
                var oldScore= context.Scores.FirstOrDefault(p => p.Id == score.Id);
                oldScore.Name = score.Name;

                await context.SaveChangesAsync();

                return true;
            }

            return false;
        }

        public async Task<bool> EditSubject(Subject subject)
        {
            if (subject != null)
            {
                var oldSubject = context.Subjects.FirstOrDefault(p => p.Id == subject.Id);
                oldSubject.Name = subject.Name;
                oldSubject.Direction = subject.Direction;

                await context.SaveChangesAsync();

                return true;
            }

            return false;
        }
    }
}