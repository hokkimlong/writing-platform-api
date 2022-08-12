namespace AuthenticationWebApi.Services.ArticleService
{
    public interface IArticleService
    {
        Task<Article> CreateArticle(CreateArticleDto article);
    }
}
