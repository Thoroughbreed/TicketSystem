using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TicketFrontend.Models;
using TicketFrontend.Service;

namespace TicketFrontend.Pages.Print;

public class Open : PageModel
{
    private readonly ITicketService _service;
    [BindProperty(SupportsGet = true)] public List<Ticket> Tickets { get; set; }
    
    public Open(ITicketService service)
    {
        _service = service;
    }
    
    public async Task<IActionResult> OnGet()
    {
        Tickets = await _service.GetTickets();
        return Page();
    }
}