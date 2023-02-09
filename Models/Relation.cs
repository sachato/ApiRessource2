namespace ApiRessource2.Models
{
    public class Relation
    {
        public int Id { get; set; }
        public int IdUser1 { get; set; }
        public int IdUser2 { get; set; }
        public DateTime date { get; set; }
        public int IdState { get; set; }
    }
}
