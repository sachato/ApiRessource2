using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApiRessource2.Models;
using ApiRessource2.Helpers;
using System;

namespace ApiRessource2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly DataContext _context;

        public CommentsController(DataContext context) p
        {
            _context = context;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Comment>> GetComment(int id)
        {
            var comment = await _context.Comments.Where(c => c.IsDeleted == false).FirstOrDefaultAsync(c => c.Id == id);

            if (comment == null)
                return NotFound("Comment not found.");

            return comment;
        }

        [HttpGet("getallcommentsbyidressource/{ressourceId}")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Comment>>> GetAllCommentsByIdRessource(int ressourceId)
        {
            var ressourceComments = await _context.Comments
                .Where(c => !c.IsDeleted && c.RessourceId == ressourceId)
                .ToListAsync();

            if (ressourceComments.Count == 0)
            {
                return NotFound("Les ressources n'ont pas été trouvées");
            }

            return ressourceComments;
        }

        [HttpGet("getallcommentsbyiduser")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Comment>>> GetAllCommentsByIdUser()
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var userId = AuthenticateResponse.GetUserIdFromToken(token);
            var userComments = await _context.Comments
                .Where(c => c.IsDeleted == false && c.UserId == userId)
                .ToListAsync();

            if (!userComments.Any())
                return NotFound("Les commentaires n'ont pas été trouvés");

            return userComments;
        }


        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutComment(int id, Comment comment)
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var userId = AuthenticateResponse.GetUserIdFromToken(token);

            if (userId == 0)
                return NotFound("L'utilisateur n'a pas été trouvé.");

            var authorizationResult = await VerifyAuthorization(comment, userId);
            if (authorizationResult != null)
                return authorizationResult;

            // Vérifier que l'id de l'entité à mettre à jour correspond à celui fourni dans l'URL
            if (id != comment.Id)
            {
                return BadRequest("L'ID fourni dans l'URL ne correspond pas à l'ID de l'entité.");
            }

            // Récupérer le commentaire correspondant à l'id dans la base de données
            var commentToUpdate = await _context.Comments.FindAsync(id);

            if (commentToUpdate == null)
            {
                return NotFound("Le commentaire n'a pas été trouvé.");
            }

            // Mettre à jour les propriétés du commentaire récupéré avec les nouvelles valeurs
            commentToUpdate.Content = comment.Content;

            _context.Entry(commentToUpdate).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {

                return NotFound();

            }

            return Ok(commentToUpdate);
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Comment>> PostComment(Comment comment)
        {
            var ressourceExists = await _context.Resources.AnyAsync(r => r.Id == comment.RessourceId);
            if (ressourceExists)
                return BadRequest("La ressource n'existe pas dans la base de données.");


            if (string.IsNullOrEmpty(comment.Content))
                return BadRequest("Le contenu du commentaire est obligatoire.");

            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var userId = AuthenticateResponse.GetUserIdFromToken(token);

            comment.DatePost = DateTime.Now;
            comment.UserId = userId;
            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();

            return Ok(comment);
        }

        [HttpPost("reply/{commentId}")]
        [Authorize]
        public async Task<ActionResult<Comment>> PostCommentReply(int commentId, Comment reply)
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var userId = AuthenticateResponse.GetUserIdFromToken(token);

            var parentComment = await _context.Comments.FindAsync(commentId);
            if (parentComment == null)
                return NotFound("Le commentaire parent n'a pas été trouvé.");

            if (string.IsNullOrEmpty(reply.Content))
                return BadRequest("Le contenu de la réponse est obligatoire.");

            var ressourceExists = await _context.Resources.AnyAsync(r => r.Id == parentComment.RessourceId);
            if (!ressourceExists)
                return BadRequest("La ressource n'existe pas dans la base de données.");

            _context.Comments.Add(new Comment
            {
                DatePost = DateTime.Now,
                Content = reply.Content,
                IsDeleted = false,
                RessourceId = parentComment.RessourceId,
                UserId = userId,
                //CommentReplyId = commentId // L'ID du commentaire parent
            });

            await _context.SaveChangesAsync();

            return Ok(reply);
        }

        [HttpDelete("{id}")]
        [Authorize(Role.Moderator)]
        public async Task<IActionResult> DeleteComment(int id)
        {
            var comment = await _context.Comments.FindAsync(id);
            if (comment == null)
                return NotFound("Le commentaire n'a pas été trouvé.");

            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var userId = AuthenticateResponse.GetUserIdFromToken(token);

            if (userId == 0)
                return NotFound("L'utilisateur n'a pas été trouvé.");

            var authorizationResult = await VerifyAuthorization(comment, userId);
            if (authorizationResult != null)
                return authorizationResult;

            comment.IsDeleted = true;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private async Task<IActionResult> VerifyAuthorization(Comment comment, int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            var isModerator = user != null && (user.Role == Role.Administrator || user.Role == Role.Moderator || user.Role == Role.SuperAdministrator);
            var isOwner = comment.UserId == userId;
            var isDeleted = !comment.IsDeleted;

            if (!isDeleted)
                return NotFound("Le commentaire que vous essayez de mettre à jour a été supprimé");

            if (!isModerator && !isOwner)
                return Unauthorized("Vous n'êtes pas autorisé à modifier ce commentaire.");

            return null;
        }
    }
}



