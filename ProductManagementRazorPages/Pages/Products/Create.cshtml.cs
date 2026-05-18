using BusinessObjects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.SignalR;
using ProductManagementRazorPages.Hubs;
using Services;

namespace ProductManagementRazorPages.Pages.Products;

public sealed class CreateModel : PageModel
{
    private readonly IProductService _contextProduct;
    private readonly ICategoryService _contextCategory;
    private readonly IHubContext<SignalRServer> _hubContext;

    public CreateModel(IProductService contextProduct, ICategoryService contextCategory, IHubContext<SignalRServer> hubContext)
    {
        _contextProduct = contextProduct;
        _contextCategory = contextCategory;
        _hubContext = hubContext;
    }

    [BindProperty]
    public Product Product { get; set; } = new();

    public IActionResult OnGet()
    {
        if (HttpContext.Session.GetString("Account") is null) return RedirectToPage("/Login");
        PopulateCategories();
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (HttpContext.Session.GetString("Account") is null) return RedirectToPage("/Login");
        if (!ModelState.IsValid)
        {
            PopulateCategories();
            return Page();
        }
        _contextProduct.SaveProduct(Product);
        await _hubContext.Clients.All.SendAsync("LoadAllItems");
        return RedirectToPage("./Index");
    }

    private void PopulateCategories() => ViewData["CategoryId"] = new SelectList(_contextCategory.GetCategories(), "CategoryId", "CategoryName");
}
