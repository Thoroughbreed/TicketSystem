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
    [BindProperty(SupportsGet = true)] public User User { get; set; }
    
    [BindProperty(SupportsGet = true)] public string Password { get; set; }
    [Compare("Password", ErrorMessage = "De to koder passer ikke, prøv lige igen!")] public string ConfirmPassword { get; set; }

    public Profile(IPropertyService pService)
    {
        _pService = pService;
    }
    public async Task<IActionResult> OnGet(int id)
    {
        User = await _pService.GetUser(id);
        return Page();
    }

    public async Task<IActionResult> OnPostEditUser()
    {
        var editedUser = new UserEditDTO();
        editedUser.password = string.IsNullOrWhiteSpace(Password) ? User.password : Password;
        editedUser.ID = User.ID;
        editedUser.display_name = User.display_name;
        editedUser.email = User.email;
        editedUser.full_name = User.full_name;
        editedUser.Created_At = User.Created_At;
        editedUser.RoleID = editedUser.RoleID;

        return RedirectToPage();
    }
}