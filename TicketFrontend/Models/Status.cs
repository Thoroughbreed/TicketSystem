using System.ComponentModel.DataAnnotations;

namespace TicketFrontend.Models;

public class Status
{
    [Key]
    public int ID { get; set; }
    public string status { get; set; }

}