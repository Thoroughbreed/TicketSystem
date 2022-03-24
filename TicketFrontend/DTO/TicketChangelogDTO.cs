namespace TicketFrontend.DTO;

public class TicketChangelogDTO
{
    public int TicketID { get; set; }
    public int UserID { get; set; }
    public DateTime EditedAt { get; set; }
    public string LogText { get; set; }
}