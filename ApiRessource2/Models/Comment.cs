using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ApiRessource2.Models
{
    public class Comment
    {
            public int Id { get; set; }
            public DateTime DatePost { get; set; }
            public string Content { get; set; }
            public bool IsDeleted { get; set; }
            public int ResourceId { get; set; }
            public int UserId { get; set; }
            public virtual Resource Resource { get; set; }
    }
}
