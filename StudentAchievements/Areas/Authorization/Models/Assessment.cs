namespace StudentAchievements.Areas.Authorization.Models
{
    public class Assessment
    {
        public int Id { get; set; }
        public Subject Subject { get; set; }
        public int Score { get; set; }
        public int StudentId { get; set; }
        public Student Student { get; set; }
    }
}
