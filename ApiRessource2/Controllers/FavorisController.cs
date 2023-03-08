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
using ApiRessource2.Helpers;
using System.Security.Claims;

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

        // GET: api/Favoris/5

        [HttpGet("getfavorisbyid/{id}")]
        [Authorize]
        public async Task<ActionResult<Favoris>> GetFavorisById(int id)
        {
            var favoris = await _context.Favoris.FindAsync(id);

            if (favoris == null)
                return NotFound();

            return Ok(favoris);

        }

        [HttpGet("getallfavorisbyiduser")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Favoris>>> GetAllFavorisByIdUser()
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var userId = AuthenticateResponse.GetUserIdFromToken(token);

            var favoris = await _context.Favoris.Where(f => f.UserId == userId).ToListAsync();

            if (favoris == null)
                return Ok(favoris);

            return favoris;
        }

        [HttpPost("{ressourceid}")]
        [Authorize]
        public async Task<ActionResult<Favoris>> PostFavoris(Favoris favoris, int RessourceId)
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var userId = AuthenticateResponse.GetUserIdFromToken(token);
            favoris.UserId = userId;
            favoris.RessourceId = RessourceId;
            _context.Favoris.Add(favoris);
            await _context.SaveChangesAsync();
            return Ok(favoris);
        }


        // DELETE: api/Favoris/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteFavoris(int id)
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var userId = AuthenticateResponse.GetUserIdFromToken(token);

            var favoris = await _context.Favoris.FindAsync(id); ;

            if (userId == null)
                return NotFound("L'utilisateur n'a pas été trouvé.");

            if (favoris == null)
                return NotFound("Le favoris que vous essayez de mettre à jour a été supprimé");

            var authorizationResult = await VerifyAuthorization(favoris, userId);
            if (authorizationResult != null)
                return authorizationResult;

            var user = await _context.Users.FindAsync(userId);
            var isOwner = _context.Favoris.Any(c => c.Id == favoris.Id && c.UserId == userId);

            if (!isOwner)
                return Unauthorized("Vous n'êtes pas autorisé à supprimer ce favoris.");

            _context.Favoris.Remove(favoris);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private async Task<IActionResult> VerifyAuthorization(Favoris favoris, int userId)
        {
            var user = await _context.Users.FindAsync(userId);

            var isModerator = user != null && (user.Role == Role.Administrator || user.Role == Role.Moderator || user.Role == Role.SuperAdministrator);
            var isOwner = favoris.UserId == userId;

            if (!isModerator && !isOwner)
                return Unauthorized("Vous n'êtes pas autorisé à modifier ce commentaire.");

            return null;
        }

    }
}
