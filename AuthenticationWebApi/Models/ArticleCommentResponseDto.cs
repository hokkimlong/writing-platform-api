namespace AuthenticationWebApi.Models
{
    public class ArticleCommentResponseDto
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public int ArticleId { get; set; }
        public DateTime CreatedAt { get; set; }
        public UserResponseDto User { get; set; }
    }
}
