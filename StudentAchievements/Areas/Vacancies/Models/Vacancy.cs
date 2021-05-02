using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel;

namespace StudentAchievements.Areas.Vacancies.Models
{
    public class Vacancy
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Salary { get; set; }
        public string Experience { get; set; }
        public string WorkType { get; set; }
        public string WorkSchedule { get; set; }
        public string Content { get;set; }
        public int EmployerId { get;set; }
        public StudentAchievements.Areas.Authorization.Models.Employer Employer { get;set; }
    }

    public static class WorkSchedules
    {
        [Description("Полный день")]
        public const string FullDay = "Полный день";
        [Description("Сменный график")]
        public const string ShiftWork = "Сменный график";
        [Description("Гибкий график")]
        public const string FlexibleWork = "Гибкий график";
        [Description("Удаленная работа")]
        public const string DistantWork = "Удаленная работа";
        [Description("Вахтовый метод")]
        public const string ShiftMethod = "Вахтовый метод";
    }

    public static class WorkTypes
    {
        [Description("Полная занятость")]
        public const string FullTime = "Полная занятость";
        [Description("Частичная занятость")]
        public const string PartTimeWork = "Частичная занятость";
        [Description("Проектная работа")]
        public const string ProjectWork = "Проектная работа";
        [Description("Волонтерство")]
        public const string Volunteering = "Волонтерство";
        [Description("Стажировка")]
        public const string Internship = "Стажировка";
    }
}