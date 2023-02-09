using APIRessource.Models;
using ApiRessource2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace APIRessource.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentaireController : ControllerBase
    {
        private readonly RessourceContext cnx;

        public CommentaireController(RessourceContext context)
        {
            cnx = context;
        }

        // POST api/<CommentaireController>CommentaireRessource
        [HttpPost]
        public void Post(Comment Comment)
        {
            Comment c = new Comment();
            var lastId = cnx.Comment.OrderByDescending(x => x.idComment).Select(x => x.idComment).FirstOrDefault();
            var userExist = cnx.USER.Any(r => r.idUser == Comment.IdUser);

            if (Comment.IdUser >= 0 && userExist)
            {
                
                c.CommentText = Comment.CommentText;
                c.IdDeleted = Comment.IdDeleted;
                c.IdRessource = Comment.IdRessource;
                c.IdUser = Comment.IdUser;
                c.DatePost = DateTime.Now; //verifier si IdRessource existe, return un objet

                cnx.Add(c);
                cnx.SaveChanges(true);
            }
        }

        // GET api/<CommentController>/5
        [HttpGet("GetCommentaires/{idCommentaire}")]
        public Comment Get(int IdComment)
        {
            return cnx.Comment.Where(c => c.IdComment == IdComment).First();
        }

        // GETALL BY RESSOURCE
        [HttpGet("GetALLCommentsByRessource/{idRessource}")]
        public IEnumerable<Comment> GetAllCommentsByRessourceId(int idRessource)
        {
            return cnx.Comment.Where(c => c.idRessource == idRessource && c.idDeleted == 0).ToList();
        }

        // DELETE api/<CommentaireController>/5
        [HttpDelete("{id}")]
        public void Delete(int id, int idUser)
        {
            var isModerator = cnx.USER.Any(r => r.idUser == idUser && (r.idRole == 8 || r.idRole == 6 || r.idRole == 7 || r.idRole == 5));
            var isOwner = cnx.COMMENTAIRE.Where(c => c.idCommentaire == id && c.idUser == idUser).Any();
            var isDeleted = cnx.COMMENTAIRE.Where(c => c.idCommentaire == id && c.idDeleted == 1).Any();
            var userRole = cnx.USER.Where(r => r.idUser == idUser).FirstOrDefault();
            COMMENTAIRE c = cnx.COMMENTAIRE.Where(c => c.idCommentaire == id).First();


            if ((isModerator || isOwner) && !isDeleted)
            {
                c.idDeleted = 1;
                c.idUser = idUser;
                cnx.Update(c);
                cnx.SaveChanges();
            }
            else if (isDeleted)
                Console.WriteLine("Le commentaire a été supprimé");
            else if (!isOwner)
                Console.WriteLine("Vous n'etes pas le propriétaire de ce commentaire");
            else if (!isModerator)
                Console.WriteLine("Vous n'etes pas le moérateur");
        }

        // PUT api/<CommentaireController>/5
        [HttpPut("{id}")]
        public void Put(int id, int idUser, [FromBody] COMMENTAIRE commentaire)
        {
            var isModerator = cnx.USER.Where(r => r.idUser == idUser && r.idRole == 8).Any();
            var isOwner = cnx.COMMENTAIRE.Where(c => c.idCommentaire == id && c.idUser == idUser).Any();
            var isDeleted = cnx.COMMENTAIRE.Where(c => c.idCommentaire == id && c.idDeleted == 1).Any();


            if ((isModerator || isOwner) && !isDeleted)
            {
                COMMENTAIRE c = cnx.COMMENTAIRE.Where(c => c.idCommentaire == id).First();
                c.commentaire = commentaire.commentaire;
                c.datePost = DateTime.Now;
                cnx.Update(c);
                cnx.SaveChanges();
            }
        }



    }

}
