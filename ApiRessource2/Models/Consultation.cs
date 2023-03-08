using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ApiRessource2.Models
{
    public class Consultation
    {
            public int Id { get; set; }
            public int RessourceId { get; set; }
            public int UserId { get; set; }
            public DateTime Date { get; set; }
    }
}

