using BusinessObjects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services;

namespace ProductManagementRazorPages.Pages.Products;

public sealed class IndexModel : PageModel
{
    private readonly IProductService _contextProduct;

    public IndexModel(IProductService contextProduct)
    {
        _contextProduct = contextProduct;
    }

    public IList<Product> Product { get; set; } = [];

    public IActionResult OnGet()
    {
        if (HttpContext.Session.GetString("Account") is null) return RedirectToPage("/Login");
        Product = _contextProduct.GetProducts();
        ViewData["Username"] = HttpContext.Session.GetString("Username");
        return Page();
    }
}
