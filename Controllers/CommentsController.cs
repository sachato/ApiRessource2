using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApiRessource2;
using ApiRessource2.Models;
using ApiRessource2.Migrations;
using System.Globalization;

namespace ApiRessource2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly DataContext _context;

        public CommentsController(DataContext context)
        {
            _context = context;
        }

        // GET: api/Comments
        [HttpGet("GetAllComment")]
        public async Task<ActionResult<IEnumerable<Comment>>> GetComments()
        {
            return await _context.Comments.Where(c => c.IsDeleted == false).ToListAsync();
        }

        // GET: api/Comments/5
        [HttpGet("GetCommentById{id}")]
        public async Task<ActionResult<Comment>> GetComment(int id)
        {
            var comment = await _context.Comments.Where(c => c.IsDeleted == false).FirstOrDefaultAsync(c => c.Id == id);

            if (comment == null)
            {
                return NotFound();
            }

            return comment;
        }

        [HttpGet("GetAllCommentsByIdRessource/{ressourceId}")]
        public async Task<ActionResult<IEnumerable<Comment>>> GetAllCommentsByIdRessource(int ressourceId)
        {
            var ressourceComments = await _context.Comments.Where(c => c.IsDeleted == false && c.RessourceId == ressourceId).ToListAsync();
            if (!ressourceComments.Any())
            {
                return NotFound();
            }

            return ressourceComments;
        }

        [HttpGet("GetAllCommentsByIdUser/{userId}")]
        public async Task<ActionResult<IEnumerable<Comment>>> GetAllCommentsByIdUser(int userId)
        {
            var userComments = await _context.Comments.Where(c => c.IsDeleted == false && c.Id == userId).ToListAsync();
            if (!userComments.Any())
            {
                return NotFound();
            }

            return userComments;
        }


        // PUT: api/Comments/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutComment(int id, Comment comment)
        {
            var isModerator = _context.Users.Where(r => r.Id == id && ((r.Role == (Role)0) || (r.Role == (Role)1) || (r.Role == (Role)2))).Any();
            var isOwner = _context.Comments.Where(c => c.Id == id && c.UserId == id).Any();
            var isDeleted = _context.Comments.Where(c => c.Id == id && c.IsDeleted == false).Any();

            if (id != comment.Id)
                return BadRequest();

            if (!isModerator || !isOwner || !isDeleted)
                return Unauthorized();

            _context.Entry(comment).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException  )
            {
            }

            return NoContent();
        }

        // POST: api/Comments
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Comment>> PostComment(Comment comment)
        {
            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetComment", new { id = comment.Id }, comment);
        }

        // DELETE: api/Comments/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteComment(Comment Comments)
        {
            var isModerator = _context.Users.Where(r => r.Id == Comments.UserId && ((r.Role == (Role)0) || (r.Role == (Role)1) || (r.Role == (Role)2))).Any();
            var isOwner = _context.Comments.Where(c => c.Id == Comments.Id && c.UserId == Comments.UserId).Any();
            var isDeleted = _context.Comments.Where(c => c.Id == Comments.Id && c.IsDeleted == false).Any();
            var comment = await _context.Comments.FindAsync(Comments.Id);
            
            if (Comments == null)
                return NotFound();

            if ((!isModerator || !isOwner) && !isDeleted)
                return Unauthorized();
            
            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
