using System.Collections.Generic;
using StudentAchievements.Areas.Authorization.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace StudentAchievements.Areas.Teacher.Models.ViewModels
{
    public class AchievementViewModel
    {
        public List<Achievement> Achievements { get;set; }

        [Required]
        [DisplayName("Название")]
        public string Name { get; set; }

        [Required]
        [DisplayName("Описание")]
        public string Description { get; set; }
        public int StudentId { get;set; }
    }
}