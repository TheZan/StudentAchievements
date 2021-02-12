using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentAchievements.Areas.Admin.Models.ViewModels
{
    public interface IEditDataViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
