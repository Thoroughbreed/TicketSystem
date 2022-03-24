using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TicketFrontend.Models;
using TicketFrontend.Service;

namespace TicketFrontend.Pages;

public class IndexModel : PageModel
{
    
    public IndexModel(ITicketService service)
    {
    }

    
    
    public void OnGet()
    {
        
    }

}