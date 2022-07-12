using System.Security.Claims;
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
    [BindProperty(SupportsGet = true)] public int ProgressBar { get; set; }
    [BindProperty(SupportsGet = true)] public string ProgressBarCol { get; set; }
    [BindProperty(SupportsGet = true)] public int UserID { get; set; }
    public List<Category> Categories { get; set; }
    public List<Priority> Priorities { get; set; }
    public List<Status> Status { get; set; }
    public List<User> Users { get; set; }

    public Detail(ITicketService service, IPropertyService propertyService)
    {
        _service = service;
        _propertyService = propertyService;
    }

    public async Task<IActionResult> OnGet(int ticketId)
    {
        if (!User.Identity.IsAuthenticated) return RedirectToPage("Account/Login");
        
        var _ul = await _propertyService.GetUsers();
        UserID = _ul.FirstOrDefault(u => u.email == User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email).Value).ID;
        FoundTicket = await _service.GetTicketByID(ticketId);
        if (FoundTicket != null)
        {
            Changelogs = await _service.GetLogs(FoundTicket.ID);
            Comment.TicketID = FoundTicket.ID;
            ProgressBar = FoundTicket.Status.status switch
            {
                "Ny" => 0,
                "Igangværende" => 25,
                "Udskudt" => 50,
                "Afvist" => 100,
                "Pause" => 50,
                "Resolved" => 90,
                "Lukket" => 100,
                "Venter på bruger" or "Afventer godkendelse" => 75,
                _ => ProgressBar
            };
            ProgressBarCol = FoundTicket.Status.status switch
            {
                "Ny" => "bg-info",
                "Igangværende" => "bg-success",
                "Udskudt" or "Pause" => "bg-warning",
                "Resolved" or "Lukket" => "bg-success",
                "Afvist" => "bg-danger",
                "Venter på bruger" or "Afventer godkendelse" => "bg-warning",
                _ => ProgressBarCol
            };
        }

        Categories = await _propertyService.GetCategories();
        Priorities = await _propertyService.GetPriority();
        Status = await _propertyService.GetStatus();
        Users = await _propertyService.GetUsers();
        
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
        await _service.CloseTicket(FoundTicket.ID, await GetUserID());
        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostReOpen()
    {
        await _service.ReOpenTicket(FoundTicket.ID, await GetUserID());
        return RedirectToPage();
    }

    private async Task<int> GetUserID()
    {
        var _ul = await _propertyService.GetUsers();
        return _ul.FirstOrDefault(u => u.email == User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email).Value).ID;
    }
}