using System.ComponentModel.DataAnnotations;

namespace ApiRessource2.Models
{
    public class ZoneGeo
    {
        public int Id { get; set; }
        public string NomCommune { get; set; }
        public int CodePostale { get; set; }
    }
}
