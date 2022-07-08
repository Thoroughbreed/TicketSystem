using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Metadata;
using TicketFrontend.Models;
using TicketFrontend.Service;

namespace TicketFrontend.Pages;

public class Tickets : PageModel
{
    private readonly ITicketService _service;
    private readonly IPropertyService _pService;
    
    [BindProperty(SupportsGet = true)] public string Search { get; set; }
    [BindProperty(SupportsGet = true)] public TicketOrderOptions orderOptions { get; set; }
    [BindProperty(SupportsGet = true)] public bool onlyMine { get; set; }

    [BindProperty(SupportsGet = true)] public List<Ticket> FoundTickets { get; set; }
    [BindProperty(SupportsGet = true)] public int CurrPage { get; set; } = 1;
    public int PageCount { get; set; }
    public int PageSize { get; set; } = 15;
    public int TotalPages => (int)Math.Ceiling(decimal.Divide(PageCount, PageSize));
    
    public Tickets(ITicketService service, IPropertyService pService)
    {
        _service = service;
        _pService = pService;
    }
    
    public async Task<IActionResult> OnGet()
    {
        if (!User.Identity.IsAuthenticated) return RedirectToPage("Account/Login");
        
        var user = await  _pService.GetUsers();
        var userID = user.FirstOrDefault(u => u.email == User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email).Value).ID;

        var pageCount = await _service.GetTicketsQ(Search);
        PageCount = pageCount.Count();
        FoundTickets  = await _service.GetTicketsQ(CurrPage, PageSize, orderOptions, onlyMine, userID, Search);
        return Page();
    }
    
    public async Task<IActionResult> OnGetCloseTaskDebug(int ticketId, int userId)
    {
        await _service.CloseTicket(ticketId, userId);
        return RedirectToPage();
    }
}