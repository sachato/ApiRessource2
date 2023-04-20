using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApiRessource2.Models;
using System.Security.Claims;
using ApiRessource2.Helpers;
using System.Linq;

namespace ApiRessource2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConsultationsController : ControllerBase
    {
        private readonly DataContext _context;

        public ConsultationsController(DataContext context)
        {
            _context = context;
        }

        // GET: api/Consultations
        [HttpGet("{id}")]
        [Authorize]
        public ActionResult<IEnumerable<Consultation>> getAllconsultationsbyid(int id)
        {
            return _context.Consultations.Where(c => c.Id == id).ToList();

        }

        // GET: api/Consultations/5
        [HttpGet("getallconsultationsbyiduser")]
        [Authorize]
        public async Task<ActionResult<Consultation>> GetConsultationById(int id)
        {
            User user = (User)HttpContext.Items["User"];
            var userId = user.Id;

            var consultation = await _context.Consultations.Where(c => c.UserId == userId).ToListAsync();
            if (consultation == null)
                return NotFound("L'utilisateur n'a pas été trouvé.");

            if (consultation == null)
                return NotFound("Le favoris que vous essayez de mettre à jour a été supprimé");

            return Ok(consultation);
            ;
        }

        // POST: api/Consultations
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("{id}")]
        [Authorize]
        public async Task<ActionResult<Consultation>> PostConsultation(int id)
        {
            User user = (User)HttpContext.Items["User"];
            var userId = user.Id;
            if (user == null)
                return NotFound("L'utilisateur n'a pas été trouvé.");

            // Vérifier si la ressource existe dans le datacontext:
            bool ressourceTrouvee = _context.Resources.Any(r => r.Id == id);

            if (ressourceTrouvee)
            {
                bool trouve = _context.Consultations.Any(c => c.RessourceId == id && c.UserId == userId);
                Consultation consultation = new Consultation();

                // Si la consultation n'existe pas déjà:
                if (!trouve)
                {
                    consultation.Date = DateTime.Now;
                    consultation.UserId = userId;
                    consultation.RessourceId = id;
                    _context.Consultations.Add(consultation);
                    await _context.SaveChangesAsync();
                    return Ok(consultation);
                }
                else
                {
                    return BadRequest("La consultation existe déjà.");
                }
            }
            else
            {
                return BadRequest("La ressource associée à l'historique n'a pas été trouvée.");
            }
        }

        // DELETE: api/Consultations/5
        [HttpDelete("{resourceId}")]
        [Authorize]
        public async Task<IActionResult> DeleteConsultation(int resourceId)
        {
            User user = (User)HttpContext.Items["User"];
            var userId = user.Id;
            if (user == null)
                return NotFound("L'utilisateur n'a pas été trouvé.");

            var consultation = await _context.Consultations.FirstOrDefaultAsync(c => c.RessourceId == resourceId);
            if (consultation == null)
                return NotFound("La consultation que vous essayez de Supprimer a deja été supprimé");

            var isModerator = user != null && (user.Role == Role.Administrator || user.Role == Role.Moderator || user.Role == Role.SuperAdministrator);
            var isOwner = _context.Consultations.Any(c => c.Id == consultation.Id && c.UserId == userId);
            if (!isModerator && !isOwner)
                return Unauthorized("Vous n'êtes pas autorisé à supprimer cette consultation.");

            _context.Consultations.Remove(consultation);
            await _context.SaveChangesAsync();

            return NoContent();
        }


        private async Task<IActionResult> VerifyAuthorization(Consultation consultation)
        {
            User user = (User)HttpContext.Items["User"];
            var userId = user.Id;
            if (userId == null)
                return NotFound("L'utilisateur n'a pas été trouvé.");

            if (consultation == null)
                return NotFound("La consultation que vous essayez de Supprimer a deja été supprimé");

            var isModerator = user != null && (user.Role == Role.Administrator || user.Role == Role.Moderator || user.Role == Role.SuperAdministrator);
            var isOwner = _context.Consultations.Any(c => c.Id == consultation.Id && c.UserId == (userId));

            if (!isModerator && !isOwner)
                return Unauthorized("Vous n'êtes pas autorisé à supprimer cette consultation.");

            return NoContent();
        }
    }
}
