namespace AuthenticationWebApi.Services.CommentService
{
    public interface ICommentService
    {
        Task<Comment> CreateComment(CreateCommentDto newComment);
    }
}
