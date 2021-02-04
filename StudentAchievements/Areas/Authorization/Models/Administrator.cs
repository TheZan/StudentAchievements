namespace StudentAchievements.Areas.Authorization.Models
{
    public class Administrator : IUser
    {
        public int Id { get; set; }
        public User User { get; set; }
    }
}