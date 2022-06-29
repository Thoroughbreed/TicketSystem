using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TicketFrontend.DTO;
using TicketFrontend.Service;

namespace TicketFrontend.Pages;

public class IndexModel : PageModel
{
    private readonly ITicketService _service;
    private readonly IPropertyService _pService;
    public int Closed { get; set; }
    public int Open { get; set; }
    public int Paused { get; set; }
    public int Resolved { get; set; }
    public bool Authed { get; set; }
    public IndexModel(ITicketService service, IPropertyService pService)
    {
        _pService = pService;
        _service = service;
    }
    
    public async Task<IActionResult> OnGet()
    {
        Authed = await CheckUser();
        if (!Authed) return Page();
        var closed = await _service.GetAllTickets();
        var open = await _service.GetTickets();
        Closed = closed.Count;
        Paused = open.Count(t => t.TStatusID is 3 or 4 or 5);
        Open = open.Count(t => t.TStatusID is 1 or 2);
        Resolved = open.Count(t => t.TStatusID is 6 or 8);

        return Page();
    }

    private async Task<bool> CheckUser()
    {
        // If no user logged in, false
        if (!User.Identity.IsAuthenticated) return false;
        
        var users = await _pService.GetUsers();
        
        // Does the user exist in the Ticket DB?
        if (users.Find(u => u.email == User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email).Value) != null)
        {
            return true;
        }
        
        var roleID = User.Claims.FirstOrDefault(c => c.Type == "https://tved.it/accessToken/roles")!.Value switch
        {
            "TicketRead" => 1,
            "TicketWrite" => 2,
            "TicketAdmin" => 3,
            _ => 0
        };
        
        if (roleID == 0) return false;
        
        var newUser = new UserDTO
        {
            Created_At = DateTime.Now,
            display_name = User.Claims.FirstOrDefault(c => c.Type == "nickname")?.Value,
            email = User.Claims.FirstOrDefault(t => t.Type == ClaimTypes.Email)?.Value,
            full_name = User.Claims.FirstOrDefault(t => t.Type == ClaimTypes.Name)?.Value,
            password = "", 
            RoleID= roleID
        };
        await _pService.CreateUser(newUser);   
        
        return false;
    }

}