namespace StudentAchievements.Areas.Authorization.Models
{
    public class Administrator : IUser
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public byte[] Photo { get; set; }
        public string Email { get; set; }
    }
}