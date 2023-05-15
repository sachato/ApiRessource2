namespace ApiRessource2.Models.Filter
{
    public enum TriType { DateDesc, DateAsc, Alphabetique, Popularité, Catégorie }
    public class Tri
    {
        public TriType TriType { get; set; }
    }
}
