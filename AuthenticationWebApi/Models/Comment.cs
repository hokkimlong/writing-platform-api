namespace AuthenticationWebApi.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string Message { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public int ArticleId { get; set; }
        public Article Article { get; set; }
    }
}
