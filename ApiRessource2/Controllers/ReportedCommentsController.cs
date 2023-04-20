using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApiRessource2;
using ApiRessource2.Models;
using ApiRessource2.Helpers;
using System.Data;

namespace ApiRessource2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportedCommentsController : ControllerBase
    {
        private readonly DataContext _context;

        public ReportedCommentsController(DataContext context)
        {
            _context = context;
        }

        // POST: api/ReportedComments
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("{id}")]
        [Authorize(Roles = new Role[] { Role.SuperAdministrator, Role.Administrator, Role.Moderator })]
        public async Task<ActionResult<ReportedComment>> PostReportedComment(int id, ReportedComment reportedComment)
        {
            User user = (User)HttpContext.Items["User"];
            if(user == null)
                return Unauthorized("Vous n'êtes pas autorisé à effectuer cette action.");

            ReportedComment newReportedComment = new();

            var comment = await _context.Comments.FindAsync(id);
            if (comment == null)
                return NotFound("Le commentaire n'a pas été trouvé.");

            var existingReport = await _context.ReportedComments
               .Where(rc => rc.CommentId == id && rc.UserId == user.Id)
               .FirstOrDefaultAsync();

            if (existingReport != null)
                return BadRequest("Vous avez déjà signalé ce commentaire.");

            newReportedComment.IsApprouved = false;
            newReportedComment.CommentId = id;
            newReportedComment.Date = DateTime.Now;
            newReportedComment.UserId = user.Id;
            newReportedComment.Reason = reportedComment.Reason;
            
                
            _context.ReportedComments.Add(newReportedComment);
            await _context.SaveChangesAsync();

            return Ok(newReportedComment);
        }

        // DELETE: api/ReportedComments/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = new Role[] { Role.SuperAdministrator, Role.Administrator, Role.Moderator })]
        public async Task<IActionResult> DeleteReportedComment(int id)
        {
            User user = (User)HttpContext.Items["User"];
            if (user == null)
                return Unauthorized("Vous n'êtes pas autorisé à effectuer cette action.");

            var reportedComment = await _context.ReportedComments.FindAsync(id);
            if (reportedComment == null)
            {
                return NotFound("Le commentaire signalé n'a pas été trouvé.");
            }

            _context.ReportedComments.Remove(reportedComment);
            await _context.SaveChangesAsync();

            return NoContent();
        }

    }
}
