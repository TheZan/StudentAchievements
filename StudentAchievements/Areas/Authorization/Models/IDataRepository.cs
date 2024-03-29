﻿using System.Linq;
using StudentAchievements.Areas.Admin.Models.ViewModels;
using System.Threading.Tasks;
using StudentAchievements.Areas.Vacancies.Models;

namespace StudentAchievements.Areas.Authorization.Models
{
    public interface IDataRepository
    {
        IQueryable<Department> Departments { get; }
        IQueryable<Direction> Directions { get; }
        IQueryable<Group> Groups { get; }
        IQueryable<Subject> Subjects { get; }
        IQueryable<Score> Scores { get; }
        IQueryable<Achievement> Achievements { get; }
        IQueryable<Assessment> Assessments { get; }
        IQueryable<Vacancy> Vacancies { get; }

        Task<bool> AddDepartment(Department department);
        Task<bool> EditDepartment(Department department);
        Task<bool> DeleteDepartment(Department department);

        Task<bool> AddDirection(Direction direction);
        Task<bool> EditDirection(Direction direction);
        Task<bool> DeleteDirection(Direction direction);

        Task<bool> AddGroup(Group group);
        Task<bool> EditGroup(Group group);
        Task<bool> DeleteGroup(Group group);

        Task<bool> AddSubject(Subject subject);
        Task<bool> EditSubject(Subject subject);
        Task<bool> DeleteSubject(Subject subject);

        Task<bool> AddScore(Score score);
        Task<bool> EditScore(Score score);
        Task<bool> DeleteScore(Score score);

        Task<bool> AddAchievement(Achievement achievement);
        Task<bool> EditAchievement(Achievement achievement);
        Task<bool> DeleteAchievement(Achievement achievement);

        Task<bool> AddAssessment(Assessment assessment);
        Task<bool> EditAssessment(Assessment assessment);
        Task<bool> DeleteAssessment(Assessment assessment);

        Task<bool> AddVacancy(Vacancy vacancy);
        Task<bool> EditVacancy(Vacancy vacancy);
        Task<bool> DeleteVacancy(Vacancy vacancy);
    }
}
