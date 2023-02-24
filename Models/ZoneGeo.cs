using System.ComponentModel.DataAnnotations;

namespace ApiRessource2.Models
{
    public class ZoneGeo
    {
        [Key]
        public string NomCommune { get; set; }
        public int CodePostale { get; set; }

    }
}
