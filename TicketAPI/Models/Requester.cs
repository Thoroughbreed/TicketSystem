using System.ComponentModel.DataAnnotations;

namespace TicketAPI.Models;

public class Requester
{
    [Key]
    public int ID { get; set; }
    public string full_name { get; set; }
    public string email { get; set; }
    public string phone { get; set; }
    public DateTime Created_At { get; set; }
}