namespace TicketFrontend.DTO;

public class TicketDTO
{
    public int ID { get; set; }
    public string TCaption { get; set; }
    public string TDesc { get; set; }

    public DateTime TCreatedAt { get; set; }
    public bool TClosed { get; set; }

    // User
    public int TCreatorID { get; set; }
    public int? TAssignedID { get; set; }
   
    public int TRequesterID { get; set; }
    
    public int TCategoryID { get; set; }
    public int TPriorityID { get; set; }
    public int TStatusID { get; set; }
}