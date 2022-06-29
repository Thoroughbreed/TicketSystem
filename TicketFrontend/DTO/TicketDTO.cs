using System.ComponentModel.DataAnnotations;

namespace TicketFrontend.DTO;

public class TicketDTO
{
    public int ID { get; set; }
    [MaxLength(25)]
    public string TCaption { get; set; }
    [MaxLength(250)]
    public string TDesc { get; set; }

    public DateTime TCreatedAt { get; set; }
    public bool TClosed { get; set; }

    // User
    public int TCreatorID { get; set; }
    // public int? TAssignedID { get; set; }
   
    // public int TRequesterID { get; set; }
    
    public int TCategoryID { get; set; }
    public int TPriorityID { get; set; }
    public int TStatusID { get; set; }
    public int? TClosedByID { get; set; }
}