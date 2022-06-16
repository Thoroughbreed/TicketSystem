using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TicketAPI.DAL;
using TicketFrontend.Models;
using TicketFrontend.Service;
using Status = TicketAPI.Models.Status;

namespace TicketFrontend.Pages;

public class IndexModel : PageModel
{
    private readonly ITicketService _service;
    public int Closed { get; set; }
    public int Open { get; set; }
    public int Paused { get; set; }
    public int Resolved { get; set; }
    public IndexModel(ITicketService service)
    {
        _service = service;
    }
    
    public async Task<IActionResult> OnGet()
    {
        var closed = await _service.GetAllTickets();
        var open = await _service.GetTickets();
        Closed = closed.Count;
        Paused = open.Count(t => t.TStatusID is 3 or 4 or 5);
        Open = open.Count(t => t.TStatusID is 1 or 2);
        Resolved = open.Count(t => t.TStatusID is 6 or 8);
        return Page();
    }

}