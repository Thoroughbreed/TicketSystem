using System.ComponentModel.DataAnnotations;

namespace TicketAPI.Models;

public class TicketChangelog
{
    [Key]
    public int ID { get; set; }
    public int TicketID { get; set; }
    public int UserID { get; set; }
    public DateTime EditedAt { get; set; }
    public string LogText { get; set; }
}