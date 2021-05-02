using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace StudentAchievements.Areas.Vacancies.Models.ViewModels
{
    public class NewVacancyViewModel
    {
        [Required]
        [Display(Name = "Название")]
        public string Name { get; set; }
        [Required]
        [Display(Name = "Заработная плата")]
        public int Salary { get; set; }
        [Required]
        [Display(Name = "Опыт работы")]
        public string Experience { get; set; }
        [Required]
        [Display(Name = "Тип занятости")]
        public string WorkType { get; set; }
        [Required]
        [Display(Name = "График работы")]
        public string WorkSchedule { get; set; }
        [Required]
        [Display(Name = "Текст вакансии")]
        public string Content { get;set; }
    }
}