using System.Text.Json.Serialization;

namespace AuthenticationWebApi.Models
{
    public class Tag
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty ;

        public List<Article> Articles { get; set; } = new List<Article>();
    }
}
