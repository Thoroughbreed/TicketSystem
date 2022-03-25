using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TicketFrontend.DTO;
using TicketFrontend.Models;
using TicketFrontend.Service;

namespace TicketFrontend.Pages;

public class Create : PageModel
{
    private readonly ITicketService _tService;
    private readonly IPropertyService _pService;
    
    [BindProperty(SupportsGet = true)] public List<Category> Categories { get; set; }
    [BindProperty(SupportsGet = true)] public List<Priority> Priorities { get; set; }
    [BindProperty(SupportsGet = true)] public List<Status> Status { get; set; }
    [BindProperty(SupportsGet = true)] public TicketDTO newTicket { get; set; }
    
    public Create(ITicketService tService, IPropertyService pService)
    {
        _tService = tService;
        _pService = pService;
    }
    public async Task<IActionResult> OnGet()
    {
        Categories = await _pService.GetCategories();
        Priorities = await _pService.GetPriority();
        Status = await _pService.GetStatus();

        return Page();
    }
    
    public async Task<IActionResult> OnPostSaveTicket()
    {
        newTicket.TRequesterID = 1; // #TODO MAKE REQUESTER FIELD THINGY
        newTicket.TCreatedAt = DateTime.Now;
        newTicket.TStatusID = 1;
        if (!ModelState.IsValid)
        {
            return Page();
        }
        var newId = await _tService.CreateTicket(newTicket);
        return RedirectToPage("Detail", new { ticketId = newId});
    }
}