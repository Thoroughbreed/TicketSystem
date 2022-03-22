﻿using System.ComponentModel.DataAnnotations;

namespace TicketAPI.Models;

public class Subscribers
{
    [Key]
    public int ID { get; set; }
    public int UserID { get; set; }
    public int TicketID { get; set; }
}