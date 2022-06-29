using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TicketFrontend.DTO;
using TicketFrontend.Models;
using TicketFrontend.Service;

namespace TicketFrontend.Pages;

public class Profile : PageModel
{
    private readonly IPropertyService _pService;
    [BindProperty(SupportsGet = true)] public User _user { get; set; }
    
    [BindProperty(SupportsGet = true)] public string Password { get; set; }
    [Compare("Password", ErrorMessage = "De to koder passer ikke, prøv lige igen!")] public string ConfirmPassword { get; set; }

    public Profile(IPropertyService pService)
    {
        _pService = pService;
    }
    public async Task<IActionResult> OnGet(int id)
    {
        if (!User.Identity.IsAuthenticated) return RedirectToPage("Account/Login");

        _user = await _pService.GetUser(id);
        return Page();
    }

    public async Task<IActionResult> OnPostEditUser()
    {
        var editedUser = new UserEditDTO();
        editedUser.password = string.IsNullOrWhiteSpace(Password) ? _user.password : Password;
        editedUser.ID = _user.ID;
        editedUser.display_name = _user.display_name;
        editedUser.email = _user.email;
        editedUser.full_name = _user.full_name;
        editedUser.Created_At = _user.Created_At;
        editedUser.RoleID = editedUser.RoleID;

        return RedirectToPage();
    }
}