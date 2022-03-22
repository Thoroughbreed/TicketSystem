using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TicketFrontend.Models;
using TicketFrontend.Service;

namespace TicketFrontend.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly ITicketService _service;

    [BindProperty(SupportsGet = true)]
    public List<Ticket> Tickets { get; set; }
    
    public IndexModel(ILogger<IndexModel> logger, ITicketService service)
    {
        _logger = logger;
        _service = service;
    }

    public async Task<IActionResult> OnGet()
    { 
        Tickets = await _service.GetTickets();
        return Page();
    }
}