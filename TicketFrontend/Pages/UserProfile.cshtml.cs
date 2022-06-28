using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace TicketFrontend.Pages;

public class UserProfile : PageModel
{
    public void OnGet()
    {
        var debug = User.Claims.FirstOrDefault(c => c.Type == "https://tved.it/accessToken/roles")!.Value;

    }
}