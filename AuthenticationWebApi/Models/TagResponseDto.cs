namespace AuthenticationWebApi.Models
{
    public class TagResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;

        public int availableArticle { get; set; } = 0;
    }
}
