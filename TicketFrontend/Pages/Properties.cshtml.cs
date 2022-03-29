using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TicketFrontend.Models;
using TicketFrontend.Service;

namespace TicketFrontend.Pages;

public class Properties : PageModel
{
    private readonly IPropertyService _pService;
    [BindProperty(SupportsGet = true)] public List<Status> Status { get; set; }
    [BindProperty(SupportsGet = true)] public List<Priority> Priorities { get; set; }
    [BindProperty(SupportsGet = true)] public List<Category> Categories { get; set; }
    [BindProperty(SupportsGet = true)] public Status NewStatus { get; set; }
    [BindProperty(SupportsGet = true)] public Priority NewPriority { get; set; }
    [BindProperty(SupportsGet = true)] public Category NewCategory { get; set; }
    
    public Properties(IPropertyService pService)
    {
        _pService = pService;
    }
    public async Task<IActionResult> OnGet()
    {
        Status = await _pService.GetStatus();
        Priorities = await _pService.GetPriority();
        Categories = await _pService.GetCategories();
        return Page();
    }

    public async Task<IActionResult> OnGetDeleteStatus(int id)
    {
        await _pService.DeleteStatus(id);
        return RedirectToPage();
    }

    public async Task<IActionResult> OnGetDeletePriority(int id)
    {
        await _pService.DeletePriority(id);
        return RedirectToPage();
    }

    public async Task<IActionResult> OnGetDeleteCategory(int id)
    {
        await _pService.DeleteCategory(id);
        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostStatus()
    {
        await _pService.CreateStatus(NewStatus);
        return RedirectToPage();
    }
    
    public async Task<IActionResult> OnPostPriority()
    {
        await _pService.CreatePriority(NewPriority);
        return RedirectToPage();
    }
    
    public async Task<IActionResult> OnPostCategory()
    {
        await _pService.CreateCategory(NewCategory);
        return RedirectToPage();
    }
    
}