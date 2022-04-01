using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TicketFrontend.DTO;
using TicketFrontend.Models;
using TicketFrontend.Service;

namespace TicketFrontend.Pages.Admin;

public class Users : PageModel
{
    private readonly IPropertyService _pService;
    [BindProperty(SupportsGet = true)] public List<User> UserList { get; set; }
    [BindProperty(SupportsGet = true)] public User User { get; set; }
    [BindProperty(SupportsGet = true)] public List<Role> Roles { get; set; }

    public Users(IPropertyService pService)
    {
        _pService = pService;
    }
    
    public async Task<IActionResult> OnGet()
    {
        UserList = await _pService.GetUsers();
        Roles = await _pService.GetRoles();
        return Page();
    }

    public async Task<IActionResult> OnGetUser(int id)
    {
        User = await _pService.GetUser(id);
        UserList = await _pService.GetUsers();
        return Page();
    }

    public async Task<IActionResult> OnPostEditModal()
    {
        var newUser = new UserDTO
        {
            Created_At = DateTime.Now,
            display_name = User.display_name,
            email = $"{User.display_name}@hartmann-packaging.com",
            full_name = User.full_name,
            password = "123456",
            RoleID = User.RoleID
        };
        
        switch (User.ID)
        {
            case > 0:
                await _pService.UpdateUser(newUser, User.ID);
                break;
            case < 1:
                await _pService.CreateUser(newUser);
                break;
        }

        return RedirectToPage();
    }
}