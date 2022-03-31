using System.ComponentModel.DataAnnotations;

namespace TicketFrontend.Models;

public class Ticket
{
    public int ID { get; set; }
    [MaxLength(25)]
    public string TCaption { get; set; }
    [MaxLength(250)]
    public string TDesc { get; set; }
    public DateTime TCreatedAt { get; set; }
    public DateTime? TClosedAt { get; set; }
    public bool TClosed { get; set; }

    // User
    public int TCreatorID { get; set; }
    public int? TAssignedID { get; set; }
    public int? TClosedByID { get; set; }
    
    public int TRequesterID { get; set; }
    
    public int TCategoryID { get; set; }
    public int TPriorityID { get; set; }
    public int TStatusID { get; set; }
    
    // Navigation properties
    public User Creator { get; set; }
    public User Asignee { get; set; }
    public User Closer { get; set; }
    public Requester Requester { get; set; }
    public Category Category { get; set; }
    public Priority Priority { get; set; }
    public Status Status { get; set; }
    //
    public ICollection<Comments> Comments { get; set; }
    public ICollection<Subscribers> Subscribers { get; set; }
}