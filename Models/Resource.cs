using System.Diagnostics.Eventing.Reader;

namespace ApiRessource2.Models
{
    public enum TypeRessource { Phonto, Lien, Texte, Document};
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
        
    }
}
