using System.ComponentModel.DataAnnotations;

namespace TicketFrontend.DTO;

public class CommentDTO
{
    public int UserID { get; set; }
    public int TicketID { get; set; }
    [MaxLength(500)]
    public string Comment { get; set; }
    public DateTime CTime { get; set; }
}