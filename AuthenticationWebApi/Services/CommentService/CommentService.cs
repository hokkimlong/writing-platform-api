namespace AuthenticationWebApi.Services.CommentService
{
    public class CommentService : ICommentService
    {
        private readonly DataContext _context;
        private readonly IAuthService _authService;
        public CommentService(DataContext context, IAuthService authService)
        {
            _context = context;
            _authService = authService;
        }

        public async Task<Comment> CreateComment(CreateCommentDto newComment)
        {
            int userId = (int)_authService.GetCurrentUserId();

            User currentUser = new User { Id = userId };
            _context.Attach(currentUser);

            Comment comment = new Comment
            {
                ArticleId = newComment.ArticleId,
                Message = newComment.Message,
                User = currentUser,
            };

            _context.Add(comment);
            _context.SaveChanges();
            return comment;
        }
    }
}
