namespace AuthenticationWebApi.Models
{
    public class CreateCommentDto
    {
        public int ArticleId { get; set; }
        public DateTime CreatedDate { get; set; } 
        public string Message { get; set; }
    }
}
