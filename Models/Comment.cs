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
            public int IdRessource { get; set; }
            public int IdUser { get; set; }
    }
}
