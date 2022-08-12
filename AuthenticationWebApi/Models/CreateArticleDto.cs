namespace AuthenticationWebApi.Models
{
    public class CreateArticleDto
    {
        public string Title { get; set; }
        public string Content { get; set; } 

        public List<Tag> Tags { get; set; } 
        public List<string> NewTags { get; set; }
    }
}
