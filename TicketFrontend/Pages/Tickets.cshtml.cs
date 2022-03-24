using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TicketFrontend.Models;
using TicketFrontend.Service;

namespace TicketFrontend.Pages;

public class Tickets : PageModel
{
    private readonly ITicketService _service;

    [BindProperty(SupportsGet = true)]
    public List<Ticket> FoundTickets { get; set; }
    
    public Tickets(ITicketService service)
    {
        _service = service;
    }
    
    public async Task<IActionResult> OnGet()
    { 
        FoundTickets = await _service.GetTickets();
        return Page();
    }
    
    
    public async Task<IActionResult> OnGetCloseTaskDebug(int ticketId, int userId)
    {
        await _service.CloseTicket(ticketId, userId);
        return Page();
    }
}