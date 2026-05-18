using BusinessObjects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Services;

namespace ProductManagementRazorPages.Pages.Products;

public sealed class EditModel : PageModel
{
    private readonly IProductService _contextProduct;
    private readonly ICategoryService _contextCategory;

    public EditModel(IProductService contextProduct, ICategoryService contextCategory)
    {
        _contextProduct = contextProduct;
        _contextCategory = contextCategory;
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
        PopulateCategories(product.CategoryId);
        return Page();
    }

    public IActionResult OnPost()
    {
        if (HttpContext.Session.GetString("Account") is null) return RedirectToPage("/Login");
        if (!ModelState.IsValid)
        {
            PopulateCategories(Product.CategoryId);
            return Page();
        }
        _contextProduct.UpdateProduct(Product);
        return RedirectToPage("./Index");
    }

    private void PopulateCategories(int selected) => ViewData["CategoryId"] = new SelectList(_contextCategory.GetCategories(), "CategoryId", "CategoryName", selected);
}
