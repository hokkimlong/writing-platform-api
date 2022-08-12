using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AuthenticationWebApi.Data;
using AuthenticationWebApi.Models;

namespace AuthenticationWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TagsController : ControllerBase
    {
        private readonly DataContext _context;

        public TagsController(DataContext context)
        {
            _context = context;
        }

        // GET: api/Tags
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TagResponseDto>>> GetTag(
            [FromQuery(Name = "page")] int page = 0,
            [FromQuery(Name = "limit")] int limit = 10
        )
        {
            string search = HttpContext.Request.Query["search"];
            string popular = HttpContext.Request.Query["popular"];

            if (_context.Tag == null)
            {
                return NotFound();
            }

            if (popular == String.Empty)
            {
                var query = _context.Tag.Select(t => new TagResponseDto { Id = t.Id, Name = t.Name });

                if (search != null)
                {
                    query = query.Where(t => t.Name.Contains(search));
                }
                return await query
                    .OrderBy(b => b.Id)
                    .Skip(page)
                    .Take(limit)
                    .ToListAsync();
            }
            else
            {
               return await _context.Tag
                    .Include(t => t.Articles)
                    .Select(
                        t =>
                            new TagResponseDto
                            {
                                Id = t.Id,
                                Name = t.Name,
                                availableArticle = t.Articles.Count
                            }
                    )
                    .Where(t => t.availableArticle > 0)
                    .ToListAsync();
            }
        }

        // GET: api/Tags/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Tag>> GetTag(int id)
        {
            if (_context.Tag == null)
            {
                return NotFound();
            }
            var tag = await _context.Tag.FindAsync(id);

            if (tag == null)
            {
                return NotFound();
            }

            return tag;
        }

        // PUT: api/Tags/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTag(int id, Tag tag)
        {
            if (id != tag.Id)
            {
                return BadRequest();
            }

            _context.Entry(tag).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TagExists(id))
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

        // POST: api/Tags
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Tag>> PostTag(Tag tag)
        {
            if (_context.Tag == null)
            {
                return Problem("Entity set 'DataContext.Tag'  is null.");
            }
            _context.Tag.Add(tag);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTag", new { id = tag.Id }, tag);
        }

        // DELETE: api/Tags/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTag(int id)
        {
            if (_context.Tag == null)
            {
                return NotFound();
            }
            var tag = await _context.Tag.FindAsync(id);
            if (tag == null)
            {
                return NotFound();
            }

            _context.Tag.Remove(tag);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TagExists(int id)
        {
            return (_context.Tag?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
