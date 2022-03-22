namespace TicketFrontend.Models;

public class User
{
    public int ID { get; set; }
    public string full_name { get; set; }
    public string display_name { get; set; }
    public string email { get; set; }
    public string password { get; set; }
    public DateTime Created_At { get; set; }
    public int RoleID { get; set; }
    
    public Role Role { get; set; }
    public ICollection<Subscribers> Subscriptions { get; set; }
}