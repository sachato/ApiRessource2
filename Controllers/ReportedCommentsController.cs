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
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<ReportedComment>> PostReportedComment(ReportedComment reportedComment)
        {
            _context.ReportedComments.Add(reportedComment);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetReportedComment", new { id = reportedComment.Id }, reportedComment);
        }
    }
}
