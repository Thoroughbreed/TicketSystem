namespace TicketFrontend.DTO;

public class UserDTO
{
    public string full_name { get; set; }
    public string display_name { get; set; }
    public string email { get; set; }
    public string password { get; set; }
    public DateTime Created_At { get; set; }
    public int RoleID { get; set; }
}