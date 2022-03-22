using System.ComponentModel.DataAnnotations;

namespace TicketAPI.Models;

public class Role
{
    [Key]
    public int ID { get; set; }
    public string Rolename { get; set; }
}