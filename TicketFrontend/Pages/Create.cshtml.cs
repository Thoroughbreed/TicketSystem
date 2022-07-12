using System.Security.Claims;
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
    [BindProperty(SupportsGet = true)] public List<User> Users { get; set; }
    [BindProperty(SupportsGet = true)] public TicketDTO newTicket { get; set; }
    
    
    public Create(ITicketService tService, IPropertyService pService)
    {
        _tService = tService;
        _pService = pService;
    }
    public async Task<IActionResult> OnGet()
    {
        if (!User.Identity.IsAuthenticated) return RedirectToPage("Account/Login");
        Categories = await _pService.GetCategories();
        Priorities = await _pService.GetPriority();
        Users = await _pService.GetUsers();

        return Page();
    }
    
    public async Task<IActionResult> OnPostSaveTicket()
    {
        newTicket.TCreatedAt = DateTime.Now;
        newTicket.TStatusID = 1;
        var _creator = await _pService.GetUsers();
        newTicket.TCreatorID = _creator.FirstOrDefault(u => u.email == User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email).Value).ID;
        ModelState.Remove("newTicket.TAssignedID");
        if (!ModelState.IsValid)
        {
            Categories = await _pService.GetCategories();
            Priorities = await _pService.GetPriority();
            Users = await _pService.GetUsers();
            
            return Page();
        }
        var newId = await _tService.CreateTicket(newTicket);
        return RedirectToPage("Detail", new { ticketId = newId});
    }
}