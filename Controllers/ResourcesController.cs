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
using NuGet.Packaging;

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
            var resource = new List<Resource>();
            try
            {
                var route = Request.Path.Value;
                var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
                resource = await _context.Resources
                   .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                   .Take(validFilter.PageSize)
                   .Where(r=>r.IsDeleted == false)
                   .ToListAsync();
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
        public async Task<IActionResult> GetResource(int id)
        {
            try
            {
                var resource = await _context.Resources.FindAsync(id);
                if (resource == null)
                {
                    string[] errorList = new string[] { $"La ressource {id} n'a pas été trouvée ou n'existe pas." };
                    Response<Resource> responseNotFound = new Response<Resource>(null, $"La ressource {id} n'a pas été trouvée ou n'existe pas.");
                    return NotFound(responseNotFound);
                }

                Response<Resource> response = new Response<Resource>(resource, $"La ressource {id} a été trouvée.");
                return Ok(response);

            }
            catch(Exception ex)
            {
                Response<Resource> response = new Response<Resource>(null, ex.Message);
                return BadRequest(response);
            }
            
        }


        // GET: api/Resources/search/dkad dazk
        [HttpGet("search/{search}")]
        public async Task<IActionResult> GetFiltredResource([FromQuery] PaginationFilter filter, string search)
        {
            var route = Request.Path.Value;
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
            var resource = await _context.Resources
                .Where(r=>r.IsDeleted == false)
                .Where(r=>r.Title.ToLower().Contains(search.ToLower()))
                .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                .Take(validFilter.PageSize)
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
        public async Task<IActionResult> PutResource(int id, Resource resource)
        {
            if (id != resource.Id)
            {
                return BadRequest("L'ID fourni dans l'URL ne correspond pas à l'ID de l'entité.");
            }

            var resourceToUpdate = await _context.Resources.FindAsync(id);

            if (resourceToUpdate == null)
            {
                return NotFound("Le commentaire n'a pas été trouvé.");
            }

            // Mettre à jour les propriétés du commentaire existant avec les nouvelles valeurs
            resourceToUpdate.Title = resource.Title;
            resourceToUpdate.Description = resource.Description;
            resourceToUpdate.CreationDate = resource.CreationDate;
            resourceToUpdate.Path = resource.Path;
            resourceToUpdate.IsDeleted = resource.IsDeleted;
            resourceToUpdate.UpVote = resource.UpVote;
            resourceToUpdate.DownVote = resource.DownVote;
            resourceToUpdate.Type = resource.Type;
            resourceToUpdate.UserId = resource.UserId;

            try
            {
                _context.Update(resourceToUpdate);
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }


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
