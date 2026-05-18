using BusinessObjects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services;

namespace ProductManagementRazorPages.Pages.Products;

public sealed class DeleteModel : PageModel
{
    private readonly IProductService _contextProduct;

    public DeleteModel(IProductService contextProduct)
    {
        _contextProduct = contextProduct;
    }

    [BindProperty]
    public Product Product { get; set; } = new();

    public IActionResult OnGet(int? id)
    {
        if (HttpContext.Session.GetString("Account") is null) return RedirectToPage("/Login");
        if (id is null) return NotFound();
        var product = _contextProduct.GetProductById(id.Value);
        if (product is null) return NotFound();
        Product = product;
        return Page();
    }

    public IActionResult OnPost(int? id)
    {
        if (HttpContext.Session.GetString("Account") is null) return RedirectToPage("/Login");
        if (id is null) return NotFound();
        var product = _contextProduct.GetProductById(id.Value);
        if (product is not null) _contextProduct.DeleteProduct(product);
        return RedirectToPage("./Index");
    }
}
