using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using StudentAchievements.Areas.Authorization.Models;

namespace StudentAchievements.Areas.Admin.Models.ViewModels
{
    public class UsersListViewModel
    {
        private UserManager<User> userManager;

        public UsersListViewModel(UserManager<User> _userManager)
        {
            userManager = _userManager;

            NotFoundUserPhoto = GetNotFoundImage();
        }

        public IEnumerable<User> Users { get; set; }

        public int CurrentPageIndex { get; set; }
        public int PageCount { get; set; }

        public byte[] NotFoundUserPhoto { get; set; }

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

        public async Task<string> GetRole(User user) => (await userManager.GetRolesAsync(user)).First();

        private byte[] GetNotFoundImage()
        {
            byte[] photo = null;

            using (var stream = new FileStream($"{Directory.GetCurrentDirectory()}/wwwroot/favicons/notFoundUserPhoto.png", FileMode.Open, FileAccess.Read))
            {
                photo = new byte[stream.Length];

                using (var reader = new BinaryReader(stream))
                {
                    photo = reader.ReadBytes((int) stream.Length);
                }
            }

            return photo;
        }
    }
}