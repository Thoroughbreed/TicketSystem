using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace TicketFrontend.Pages;

public class UserProfile : PageModel
{
    public IActionResult OnGet()
    {
        if (!User.Identity.IsAuthenticated) return RedirectToPage("Account/Login");

        var debug = User.Claims.FirstOrDefault(c => c.Type == "https://tved.it/accessToken/roles")!.Value;
        return Page();

    }
}