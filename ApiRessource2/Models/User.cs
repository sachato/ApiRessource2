using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ApiRessource2.Models
{
    public enum Role { User, Moderator , Administrator, SuperAdministrator };
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }

        [JsonIgnore]
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime CreationDate { get; set; }
        public bool IsConfirmed { get; set; }
        public bool IsDeleted { get; set; }
        public Role Role { get; set; }
        public int ZoneGeoId { get; set; }

        //[ForeignKey("UserId")]
        //public virtual ICollection<Resource> Resource { get; set; }

    }
}