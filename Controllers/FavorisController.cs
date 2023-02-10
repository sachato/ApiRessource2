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

namespace ApiRessource2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FavorisController : ControllerBase
    {
        private readonly DataContext _context;

        public FavorisController(DataContext context)
        {
            _context = context;
        }

            // GET: api/Favoris
            [HttpGet("GetAllFavoris/")]
            public async Task<ActionResult<IEnumerable<Favoris>>> GetAllFavoris()
            {
                return await _context.Favoris.ToListAsync();
            }

            // GET: api/Favoris/5
            [HttpGet("GetFavorisById/{id}")]
            public async Task<ActionResult<Favoris>> GetFavorisById(int id)
            {
                var favoris = await _context.Favoris.FindAsync(id);

                if (favoris == null)
                {
                    return NotFound();
                }

                return favoris;
            }

            [HttpGet("GetAllFavorisByIdUser/{idUser}")]
            public async Task<ActionResult<IEnumerable<Favoris>>> GetAllFavorisByIdUser(int idUser)
            {
                var favoris = await _context.Favoris.Where(f => f.  UserId == idUser).ToListAsync();

                if (favoris == null)
                {
                    return NotFound();
                }

                return favoris;
            }

        [HttpPost]
        public async Task<ActionResult<Favoris>> PostFavoris(Favoris favoris)
        {
            _context.Favoris.Add(favoris);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetFavorisById), new { id = favoris.Id }, favoris);
        }


        // DELETE: api/Favoris/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFavoris(int id, int idUser)
        {
            var favoris = await _context.Favoris.FindAsync(id);
            var isOwner = _context.Comments.Where(c => c.Id == id && c.UserId == idUser).Any();


            if (favoris == null)           
                return NotFound();


                if (!isOwner)
                    return Unauthorized();

            _context.Favoris.Remove(favoris);
            await _context.SaveChangesAsync();

            return NoContent();
        }

    }
}
