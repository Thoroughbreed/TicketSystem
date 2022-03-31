using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TicketFrontend.Models;
using TicketFrontend.Service;

namespace TicketFrontend.Pages;

public class Tickets : PageModel
{
    private readonly ITicketService _service;

    [BindProperty(SupportsGet = true)] public List<Ticket> FoundTickets { get; set; }
    [BindProperty(SupportsGet = true)] public int CurrPage { get; set; } = 1;
    public int PageCount { get; set; }
    public int PageSize { get; set; } = 15;
    public int TotalPages => (int)Math.Ceiling(decimal.Divide(PageCount, PageSize));
    
    public Tickets(ITicketService service)
    {
        _service = service;
    }
    
    public async Task<IActionResult> OnGet()
    {
        var pageCount = await _service.GetTickets();
        PageCount = pageCount.Count;
        FoundTickets = await _service.GetTicketsQ(CurrPage, PageSize);
        return Page();
    }
    
    public async Task<IActionResult> OnGetCloseTaskDebug(int ticketId, int userId)
    {
        await _service.CloseTicket(ticketId, userId);
        return RedirectToPage();
    }
}