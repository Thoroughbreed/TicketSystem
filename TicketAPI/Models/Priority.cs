using System.ComponentModel.DataAnnotations;

namespace TicketAPI.Models;

public class Priority
{
    [Key]
    public int ID { get; set; }
    public string priority { get; set; }
    
}