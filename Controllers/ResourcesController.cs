using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApiRessource2;
using ApiRessource2.Models;
using ApiRessource2.Models.Wrapper;
using ApiRessource2.Models.Filter;
using ApiRessource2.Helpers;
using ApiRessource2.Services;

namespace ApiRessource2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResourcesController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IUriService uriService;

        public ResourcesController(DataContext context, IUriService uriService)
        {
            _context = context;
            this.uriService = uriService;
        }

        // GET: api/Resources
        [HttpGet]
        public async Task<IActionResult> GetResources([FromQuery] PaginationFilter filter, TriType triType)
        {
            var resource = new List<Resource>();
            try
            {
                var route = Request.Path.Value;
                var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
                var query = _context.Resources
                   .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                   .Take(validFilter.PageSize)
                   .Include(r => r.User)
                   .AsQueryable();

                if(triType == TriType.Alphabetique)
                {
                    resource = await query.OrderBy(q => q.Title).ToListAsync();
                }
                if (triType == TriType.Popularité)
                {
                    resource = await query.OrderByDescending(q => q.UpVote).ToListAsync();
                }
                if (triType == TriType.DateAsc)
                {
                    resource = await query.OrderBy(q => q.CreationDate.Date).ThenBy(q=>q.CreationDate.TimeOfDay).ToListAsync();
                }
                if (triType == TriType.DateDesc)
                {
                    resource = await query.OrderByDescending(q => q.CreationDate).ThenBy(q => q.CreationDate.TimeOfDay).ToListAsync();
                }


                var totalRecords = await _context.Resources.CountAsync();
                var pagedReponse = PaginationHelper.CreatePagedReponse<Resource>(resource, validFilter, totalRecords, uriService, route);
                return Ok(pagedReponse);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
            
        }

        // GET: api/Resources/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Resource>> GetResource(int id)
        {
            var resource = await _context.Resources.Where(r=>r.Id == id).Include(r => r.User).FirstOrDefaultAsync();
            if (resource == null)
            {
                return NotFound();
            }

            return Ok(resource);
        }


        // GET: api/Resources/search/dkad dazk
        [HttpGet("search/{search}")]
        public async Task<IActionResult> GetFiltredResource([FromQuery] PaginationFilter filter, string search)
        {
            var route = Request.Path.Value;
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
            var resource = await _context.Resources
                .Where(r=>r.Title.ToLower().Contains(search.ToLower()))
                .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                .Take(validFilter.PageSize)
                .Include(r => r.User)
                .ToListAsync();

            if (resource == null)
            {
                return NotFound();
            }

            var totalRecords = await _context.Resources.CountAsync();
            var pagedReponse = PaginationHelper.CreatePagedReponse<Resource>(resource, validFilter, totalRecords, uriService, route);
            return Ok(pagedReponse);
        }


        // PUT: api/Resources/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutComment(int id, Comment comment)
        {
            if (id != comment.Id)
            {
                return BadRequest("L'ID fourni dans l'URL ne correspond pas à l'ID de l'entité.");
            }

            var commentToUpdate = await _context.Comments.FindAsync(id);

            if (commentToUpdate == null)
            {
                return NotFound("Le commentaire n'a pas été trouvé.");
            }

            // Mettre à jour les propriétés du commentaire existant avec les nouvelles valeurs
            commentToUpdate.Content = comment.Content;
            commentToUpdate.DatePost = DateTime.Now;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {

            }

            return NoContent();
        }


        // POST: api/Resources
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Resource>> PostResource(Resource resource)
        {
            _context.Resources.Add(resource);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetResource", new { id = resource.Id }, resource);
        }

        // DELETE: api/Resources/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteResource(int id)
        {
            var resource = await _context.Resources.FindAsync(id);
            if (resource == null)
            {
                return NotFound();
            }

            _context.Resources.Remove(resource);
            await _context.SaveChangesAsync();

            return NoContent();
        }


        // PUT: api/Resources/upvote/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("upvote")]
        public async Task<IActionResult> upvote(int idresource, int iduser)
        {
            if (idresource == null || idresource == 0 || iduser == null || iduser == 0)
            {
                return BadRequest("La ressource est introuvable.");
            }

            var voted = await _context.Voteds.Where(v => v.UserId == iduser).Where(v => v.RessourceId == idresource).FirstOrDefaultAsync();
            if(voted == null)
            {
                var resourceToUpdate = await _context.Resources.FindAsync(idresource);

                if (resourceToUpdate == null)
                {
                    return NotFound("La ressource est introuvable.");
                }

                resourceToUpdate.UpVote++;
                voted = new Voted(){};
                voted.RessourceId = idresource;
                voted.UserId = iduser;
                _context.Voteds.Add(voted);
                _context.Update(resourceToUpdate);



                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {

                }
            }
            else
            {
                return BadRequest("Vous avez deja aimé cette ressource.");
            }

            

            return NoContent();
        }



        // PUT: api/Resources/upvote/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("downvote/{id}")]
        public async Task<IActionResult> downvote(int idresource, int iduser)
        {
            if (idresource == null || idresource == 0 || iduser == null || iduser == 0)
            {
                return BadRequest("La ressource est introuvable.");
            }

            var voted = await _context.Voteds.Where(v => v.UserId == iduser).Where(v => v.RessourceId == idresource).FirstOrDefaultAsync();
            if (voted == null)
            {
                var resourceToUpdate = await _context.Resources.FindAsync(idresource);

                if (resourceToUpdate == null)
                {
                    return NotFound("La ressource est introuvable.");
                }

                resourceToUpdate.DownVote++;
                voted = new Voted() { };
                voted.RessourceId = idresource;
                voted.UserId = iduser;
                _context.Voteds.Add(voted);
                _context.Update(resourceToUpdate);



                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {

                }
            }
            else
            {
                return BadRequest("Vous avez deja aimé cette ressource.");
            }



            return NoContent();
        }


        private bool ResourceExists(int id)
        {
            return _context.Resources.Any(e => e.Id == id);
        }
    }
}
