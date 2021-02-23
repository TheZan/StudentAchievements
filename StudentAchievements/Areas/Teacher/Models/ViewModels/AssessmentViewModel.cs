using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using StudentAchievements.Areas.Authorization.Models;

namespace StudentAchievements.Areas.Teacher.Models.ViewModels
{
    public class AssessmentViewModel
    {
        public int StudentId { get;set; }
        public int AssessmentId { get;set; }
        public string SubjectName { get;set; }
        public int SubjectGrade { get;set; }
        public int SubjectSemester { get;set; }
        public string ControlTypeName { get;set; }
        public int Score { get;set; }
        public IEnumerable<SelectListItem> ScoreList { get; set; }
    }
}