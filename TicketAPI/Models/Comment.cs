using System.ComponentModel.DataAnnotations;

namespace TicketAPI.Models;

public class Comments
{
    [Key]
    public int ID { get; set; }
    public int UserID { get; set; }
    public int TicketID { get; set; }
    public string Comment { get; set; }
    public DateTime CTime { get; set; }

    public User User { get; set; }
}