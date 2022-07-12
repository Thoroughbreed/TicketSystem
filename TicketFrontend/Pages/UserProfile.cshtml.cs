﻿using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace TicketFrontend.Pages;

public class UserProfile : PageModel
{
    public IActionResult OnGet()
    {
        if (User.Identity == null || !User.Identity.IsAuthenticated) return RedirectToPage("Account/Login");

        var debug = User.Claims.FirstOrDefault(c => c.Type == "https://hartmann-group.net/accessToken/roles")!.Value;
        return Page();
    }
}