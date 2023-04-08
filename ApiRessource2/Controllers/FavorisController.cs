﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApiRessource2;
using ApiRessource2.Models;
using ApiRessource2.Helpers;
using System.Security.Claims;
using OpenQA.Selenium.DevTools;

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
        public async Task<ActionResult<Favoris>> GetAllFavorisByIdUser()
        {
            User user = (User)HttpContext.Items["User"];
            var userId = user.Id;
            var favoris = await _context.Favoris.FirstOrDefaultAsync(f => f.UserId == userId);

        if (favoris == null)
                return Ok(favoris);

            return Ok(favoris);
        }

        [HttpPost("{id}")]
        [Authorize]
        public async Task<ActionResult<Favoris>> PostFavoris(int id)
        {
            User user = (User)HttpContext.Items["User"];
            var userId = user.Id;

            //verifier si l'utilisateur a déjà ajouté cette ressource à ses favoris
            if (_context.Favoris.Where(f => f.UserId == userId && f.ResourceId == id).FirstOrDefault() != null)
                BadRequest("Vous avez déjà ajouté cette ressource à vos favoris.");

            Resource resource = new Resource();
            Favoris favoris = new Favoris();
            if (_context.Resources.Any(r => r.Id == id))
                {
                    favoris = new Favoris()
                    {
                        ResourceId = id,
                        UserId = userId
                    };
                    _context.Favoris.Add(favoris);
                    await _context.SaveChangesAsync();
                    return Ok(favoris);

                }           
            

            return NotFound("La ressource n'a pas été trouvée.");

        }


        // DELETE: api/Favoris/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteFavoris(int id)
        {
            User user = (User)HttpContext.Items["User"];
            var userId = user.Id;

            var favoris = await _context.Favoris.FindAsync(id); ;

            if (userId == null)
                return NotFound("L'utilisateur n'a pas été trouvé.");

            if (favoris == null)
                return NotFound("Le favoris que vous essayez de mettre à jour a été supprimé");

            var authorizationResult = await VerifyAuthorization(favoris);
            if (authorizationResult != null)
                return authorizationResult;

            var isOwner = _context.Favoris.Any(c => c.Id == favoris.Id && c.UserId == userId);

            if (!isOwner)
                return Unauthorized("Vous n'êtes pas autorisé à supprimer ce favoris.");

            _context.Favoris.Remove(favoris);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private async Task<IActionResult> VerifyAuthorization(Favoris favoris)
        {
            User user = (User)HttpContext.Items["User"];
            var userId = user.Id;
            var isModerator = user != null && (user.Role == Role.Administrator || user.Role == Role.Moderator || user.Role == Role.SuperAdministrator);
            var isOwner = favoris.UserId == userId;

            if (!isModerator && !isOwner)
                return Unauthorized("Vous n'êtes pas autorisé à modifier ce commentaire.");

            return null;
        }

    }
}
