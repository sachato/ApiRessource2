using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.Eventing.Reader;

namespace ApiRessource2.Models
{
    public enum TypeRessource { Photo, Lien, Texte, Document};
    public class Resource
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreationDate { get; set; }
        public string Path { get; set; }
        public bool IsDeleted { get; set; }
        public int UpVote { get; set; }
        public int DownVote { get; set; }
        public TypeRessource Type { get; set; }
        public int UserId { get; set; }
        public virtual UserReturn User { get; set; }

        [ForeignKey("ResourceId")]
        public virtual ICollection<Comment> Comments { get; set; }

        //[ForeignKey("ResourceId")]
        public virtual Voted Voted { get; set; }

        public virtual Favoris Favoris { get; set; }

    }
}
