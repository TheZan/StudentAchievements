namespace StudentAchievements.Areas.Authorization.Models
{
    public class Assessment
    {
        public int Id { get; set; }
        public Subject Subject { get; set; }
        public int Score { get; set; }
    }
}
