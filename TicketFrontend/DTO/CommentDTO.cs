namespace TicketFrontend.DTO;

public class CommentDTO
{
    public int UserID { get; set; }
    public int TicketID { get; set; }
    public string Comment { get; set; }
    public DateTime CTime { get; set; }
}