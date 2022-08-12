using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AuthenticationWebApi.Data;
using AuthenticationWebApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace AuthenticationWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticlesController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IArticleService _articleService;
        public ArticlesController(DataContext context,IArticleService articleService)
        {
            _context = context;
            _articleService = articleService; 
            
        }

        // GET: api/Articles
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ArticleResponseDto>>> GetArticle(
            [FromQuery(Name = "page")] int page = 0,
            [FromQuery(Name = "limit")] int limit = 100
            )
        {
           string search = HttpContext.Request.Query["search"];
            string tag = HttpContext.Request.Query["tag"];
            string user = HttpContext.Request.Query["user"];
           

          if (_context.Article == null)
          {
              return NotFound();
          }

            var query = _context.Article.Include(a => a.User).Include(a => a.Tags).Select(a => new ArticleResponseDto
            {
                Id = a.Id,
                Title = a.Title,
                User = new UserResponseDto { Name = a.User.Name, Id = a.User.Id, Email = a.User.Email },
                Tags = a.Tags,
                CreatedAt = a.CreatedDate
            });

            if (search != "")
            {
                query = query.Where(t => t.Title.Contains(search) || t.Tags.Any(t=>t.Name.Contains(search)));
            }else if(user != null)
            {
                query = query.Where(a => a.User.Id == int.Parse(user));
            }else if(tag != null)
            {
                query = query.Where(a => a.Tags.Any(t => t.Id == int.Parse(tag)));
            }

            return await query.OrderByDescending(b => b.CreatedAt).Skip(page).Take(limit).ToListAsync();
                
        }

        // GET: api/Articles/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ArticleResponseDto>> GetArticle(int id)
        {
          if (_context.Article == null)
          {
              return NotFound();
          }
            var article = await _context.Article
                .Include(a => a.User)
                .Include(a => a.Tags)
                .Select(a => new ArticleResponseDto
            {
                Id = a.Id,
                Title = a.Title,
                User = new UserResponseDto { Name = a.User.Name, Id = a.User.Id, Email = a.User.Email },
                Tags = a.Tags,
                Content = a.Content,
                CreatedAt = a.CreatedDate
            }).Where(t => t.Id == id).FirstAsync();

            
            if (article == null)
            {
                return NotFound();
            }

            return article;
        }

        // GET: api/Articles/5
        [HttpGet("{id}/comment")]
        public async Task<ActionResult<IEnumerable<ArticleCommentResponseDto>>>  GetComment(int id)
        {

            if (_context.Article == null)
            {
                return NotFound();
            }
            return await _context.Comment
                .Where(a => a.ArticleId == id)
                .Include(a => a.User)
                .Select(a => new ArticleCommentResponseDto
                {
                    Id = a.Id,
                    Message = a.Message,
                    User = new UserResponseDto { Name = a.User.Name, Id = a.User.Id, Email = a.User.Email },
                    CreatedAt = a.CreatedDate,
                    ArticleId = a.ArticleId
                }).OrderBy(b => b.CreatedAt).ToListAsync();

        }

        // PUT: api/Articles/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutArticle(int id, Article article)
        {
            if (id != article.Id)
            {
                return BadRequest();
            }

            _context.Entry(article).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ArticleExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Articles
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        public async Task<ActionResult<Article>> PostArticle(CreateArticleDto article)
        {
          if (_context.Article == null)
          {
              return Problem("Entity set 'DataContext.Article'  is null.");
          }
            var newArticle = await  _articleService.CreateArticle(article);

            return CreatedAtAction("GetArticle", new { id = newArticle.Id }, article);
        }

        // DELETE: api/Articles/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteArticle(int id)
        {
            if (_context.Article == null)
            {
                return NotFound();
            }
            var article = await _context.Article.FindAsync(id);
            if (article == null)
            {
                return NotFound();
            }

            _context.Article.Remove(article);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ArticleExists(int id)
        {
            return (_context.Article?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
