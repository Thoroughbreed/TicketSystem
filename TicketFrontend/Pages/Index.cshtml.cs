using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TicketFrontend.Models;
using TicketFrontend.Service;

namespace TicketFrontend.Pages;

public class IndexModel : PageModel
{
    private readonly ITicketService _service;

    [BindProperty(SupportsGet = true)]
    public List<Ticket> Tickets { get; set; }
    public Comments Comment { get; set; }
    
    public IndexModel(ITicketService service)
    {
        _service = service;
    }

    public async Task<IActionResult> OnGet()
    { 
        Tickets = await _service.GetTickets();
        return Page();
    }

    public async Task<IActionResult> OnGetCloseTaskDebug(int ticketId, int userId)
    {
        await _service.CloseTicket(ticketId, userId);
        return Page();
    }

    public async Task OnPostComment(Comments comment)
    {
        await _service.CreateComment(comment);
    }
}