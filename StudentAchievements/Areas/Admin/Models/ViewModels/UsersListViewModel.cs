using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using StudentAchievements.Areas.Authorization.Models;

namespace StudentAchievements.Areas.Admin.Models.ViewModels
{
    public class UsersListViewModel
    {
        public IEnumerable<IdentityUser> IdentityUsers { get; set; }
        public IEnumerable<IUser> ApplicationUsers { get; set; }

        public int CurrentPageIndex { get; set; }
        public int PageCount { get; set; }

        public List<int> GetPageList(int startIndex, int endIndex)
        {
            var indexList = new List<int>();

            for (int i = startIndex; i <= endIndex; i++)
            {
                if (i <= PageCount)
                {
                    indexList.Add(i);
                }
            }

            return indexList;
        }
    }
}