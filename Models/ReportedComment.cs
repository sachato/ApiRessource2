namespace ApiRessource2.Models
{
    public class ReportedComment
    {
        public int Id { get; set; }
        public int IdComment { get; set; }
        public DateTime Date { get; set; }
        public string? Motif { get; set; }
        public bool IsApprouved { get; set; }
        public int IdModerator { get; set; }
    }
}
