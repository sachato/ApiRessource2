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
        public async Task<IActionResult> GetResources([FromQuery] PaginationFilter filter)
        {
            var route = Request.Path.Value;
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
            var resource = await _context.Resources.Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
               .Take(validFilter.PageSize)
               .ToListAsync();
            var totalRecords = await _context.Resources.CountAsync();
            var pagedReponse = PaginationHelper.CreatePagedReponse<Resource>(resource, validFilter, totalRecords, uriService, route);
            return Ok(pagedReponse);
        }

        // GET: api/Resources/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Resource>> GetResource(int id)
        {
            var resource = await _context.Resources.FindAsync(id);
            /* var resource = _context.Resources.Where(e=>e.Id == id).FirstOrDefault();*/
            /*var resource = new Resource { Id = 1, Title="ta mere",  Description="la pute", CreationDate = DateTime.Now, Path = "Path", DownVote = 1, UpVote = 2, IdUser = 32, IsDeleted=false, Type=TypeRessource.Lien};*/

            if (resource == null)
            {
                return NotFound();
            }

            return resource;
        }


        // GET: api/Resources/search/dkad dazk
        [HttpGet("search/{search}")]
        public async Task<ActionResult<IEnumerable<Resource>>> GetFiltredResource(string search)
        {
            //var resource = await _context.Resources.FindAsync(id);
            var resource = await _context.Resources.Where(r=>r.Title.Contains(search)).ToListAsync();
            /* var resource = _context.Resources.Where(e=>e.Id == id).FirstOrDefault();*/
            /*var resource = new Resource { Id = 1, Title="ta mere",  Description="la pute", CreationDate = DateTime.Now, Path = "Path", DownVote = 1, UpVote = 2, IdUser = 32, IsDeleted=false, Type=TypeRessource.Lien};*/

            if (resource == null)
            {
                return NotFound();
            }

            return resource;
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

        private bool ResourceExists(int id)
        {
            return _context.Resources.Any(e => e.Id == id);
        }
    }
}
