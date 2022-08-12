using System.ComponentModel.DataAnnotations;

namespace AuthenticationWebApi.Models
{
    public class Article
    {
        public int Id { get; set; }
        public string Title { get; set; }  = string.Empty;
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public string Content { get; set; }= string.Empty;
        public int UserId { get; set; }
        public  User User { get; set; } = new User();

        public List<Tag> Tags { get; set; }

    }
}
