namespace AuthenticationWebApi.Services.ArticleService
{
    public class ArticleService : IArticleService
    {
        private readonly DataContext _context;
        private readonly IAuthService _authService;
        public ArticleService(DataContext context, IAuthService authService)
        {
            _context = context;
            _authService = authService;
        }

        public async Task<Article> CreateArticle(CreateArticleDto article)
        {
            int userId = (int)_authService.GetCurrentUserId();

            User currentUser = new User { Id = userId };
            _context.Attach(currentUser);

            List<Tag> tags = new List<Tag>();
            foreach (Tag tag in article.Tags)
            {
                Tag currentTag = new Tag() { Id = tag.Id }; 
                tags.Add(currentTag);
                _context.Attach(currentTag);
            }

            foreach (string newTag in article.NewTags)
            {
                tags.Add(new Tag { Name = newTag });
            }

            Article newArticle = new Article()
            {
                Title = article.Title,
                Content = article.Content,
                User = currentUser,
                Tags = tags,
            };

            _context.Article.Add(newArticle);

            await _context.SaveChangesAsync();

            return new Article();
        }


    }
}
