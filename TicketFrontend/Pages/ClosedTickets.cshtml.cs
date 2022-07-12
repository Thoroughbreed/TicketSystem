using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TicketFrontend.Models;
using TicketFrontend.Service;

namespace TicketFrontend.Pages;

public class ClosedTickets : PageModel
{
    private readonly ITicketService _service;

    [BindProperty(SupportsGet = true)] public string Search { get; set; }
    [BindProperty(SupportsGet = true)] public TicketOrderOptions orderOptions { get; set; }
    
    
    [BindProperty(SupportsGet = true)] public List<Ticket> FoundTickets { get; set; }
    [BindProperty(SupportsGet = true)] public int CurrPage { get; set; } = 1;
    public int PageCount { get; set; }
    public int PageSize { get; set; } = 15;
    public int TotalPages => (int)Math.Ceiling(decimal.Divide(PageCount, PageSize));

    public ClosedTickets(ITicketService service)
    {
        _service = service;
    }
    public async Task<IActionResult> OnGet()
    {
        if (!User.Identity.IsAuthenticated) return RedirectToPage("Account/Login");
        
        var pageCount = await _service.GetTicketsQ(Search);
        PageCount = pageCount.Where(t => t.TClosed).Count();
        
        FoundTickets = await _service.GetClosedTicketsQ(CurrPage, PageSize, orderOptions, Search);
        return Page();
    }
}