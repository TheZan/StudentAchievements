using System.IO;

namespace StudentAchievements.Infrastructure
{
    public static class NotFoundImageUtility
    {
        public static byte[] GetNotFoundImage()
        {
            byte[] photo = null;

            using (var stream = new FileStream($"{Directory.GetCurrentDirectory()}/wwwroot/favicons/notFoundUserPhoto.png", FileMode.Open, FileAccess.Read))
            {
                photo = new byte[stream.Length];

                using (var reader = new BinaryReader(stream))
                {
                    photo = reader.ReadBytes((int)stream.Length);
                }
            }

            return photo;
        }
    }
}