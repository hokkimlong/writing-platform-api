namespace AuthenticationWebApi.Models
{
    public class ArticleResponseDto
    {
        public int Id { get; set; }
        public string Title { get; set; }= String.Empty;
        public string Content { get; set; } = String.Empty;
        public UserResponseDto User { get; set; } = new UserResponseDto();

        public DateTime CreatedAt { get; set; }

        public List<Tag> Tags { get; set; } 
    }
}
