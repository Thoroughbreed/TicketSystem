using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TicketFrontend.Models;
using TicketFrontend.Service;

namespace TicketFrontend.Pages;

public class Detail : PageModel
{
    private readonly ITicketService _service;

    [BindProperty(SupportsGet = true)] public Ticket FoundTicket { get; set; }
    [BindProperty(SupportsGet = true)] public List<TicketChangelog> Changelogs { get; set; }
    [BindProperty] public Comments Comment { get; set; } = new();


    public Detail(ITicketService service)
    {
        _service = service;
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
}