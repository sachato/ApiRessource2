namespace ApiRessource2.Models
{
    public class ReportedComment
    {
        public int Id { get; set; }
        public int CommentId { get; set; }
        public DateTime Date { get; set; }
        public string Reason { get; set; }
        public bool IsApprouved { get; set; }
        public int UserId { get; set; }
    }
}
