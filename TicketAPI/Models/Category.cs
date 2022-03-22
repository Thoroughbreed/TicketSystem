using System.ComponentModel.DataAnnotations;

namespace TicketAPI.Models;

public class Category
{
    [Key]
    public int ID { get; set; }
    public string category { get; set; }
}