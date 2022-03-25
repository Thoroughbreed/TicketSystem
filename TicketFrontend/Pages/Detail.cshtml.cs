using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TicketFrontend.Models;
using TicketFrontend.Service;

namespace TicketFrontend.Pages;

public class Detail : PageModel
{
    private readonly ITicketService _service;
    private readonly IPropertyService _propertyService;

    [BindProperty(SupportsGet = true)] public Ticket FoundTicket { get; set; }
    [BindProperty(SupportsGet = true)] public List<TicketChangelog> Changelogs { get; set; }
    [BindProperty] public Comments Comment { get; set; } = new();
    public List<Category> Categories { get; set; }
    public List<Priority> Priorities { get; set; }
    public List<Status> Status { get; set; }

    public Detail(ITicketService service, IPropertyService propertyService)
    {
        _service = service;
        _propertyService = propertyService;
    }

    public async Task<IActionResult> OnGet(int ticketId)
    {
        FoundTicket = await _service.GetTicketByID(ticketId);
        if (FoundTicket != null)
        {
            Changelogs = await _service.GetLogs(FoundTicket.ID);
            Comment.TicketID = FoundTicket.ID;
            Comment.UserID = FoundTicket.TCreatorID;
        }

        Categories = await _propertyService.GetCategories();
        Priorities = await _propertyService.GetPriority();
        Status = await _propertyService.GetStatus();
        
        return Page();
    }

    public async Task<IActionResult> OnPostSaveTicket()
    {
        await _service.EditTicket(FoundTicket);
        return RedirectToPage();
    }
    
    public async Task<IActionResult> OnPostCreateComment(Comments comment)
    {
        await _service.CreateComment(comment);
        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostCloseTicket()
    {
        await _service.CloseTicket(FoundTicket.ID, 5); // #TODO HARDCODED USER VALUE
        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostReOpen()
    {
        await _service.ReOpenTicket(FoundTicket.ID, 4); // #TODO HARDCODED USER VALUE
        return RedirectToPage();
    }
}