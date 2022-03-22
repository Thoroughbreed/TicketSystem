using System.ComponentModel.DataAnnotations;

namespace TicketAPI.Models;

public class Status
{
    [Key]
    public int ID { get; set; }
    public string status { get; set; }

}