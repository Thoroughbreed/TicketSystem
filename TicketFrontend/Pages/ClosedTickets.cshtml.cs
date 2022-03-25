using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TicketFrontend.Models;
using TicketFrontend.Service;

namespace TicketFrontend.Pages;

public class ClosedTickets : PageModel
{
    private readonly ITicketService _service;

    [BindProperty(SupportsGet = true)]
    public List<Ticket> FoundTickets { get; set; }

    public ClosedTickets(ITicketService service)
    {
        _service = service;
    }
    public async Task<IActionResult> OnGet()
    {
        FoundTickets = await _service.GetAllTickets();
        return Page();
    }
}