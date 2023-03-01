namespace ApiRessource2.Models.Filter
{
    public enum TriType { DateDesc, DateAsc, Alphabetique, Popularité }
    public class Tri
    {
        public TriType TriType { get; set; }
    }
}
