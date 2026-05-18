using BusinessObjects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services;

namespace ProductManagementRazorPages.Pages;

public sealed class LoginModel : PageModel
{
    private readonly IAccountService _accountService;

    public LoginModel(IAccountService accountService)
    {
        _accountService = accountService;
    }

    [BindProperty]
    public AccountMember AccountMember { get; set; } = new();

    public IActionResult OnGet()
    {
        return HttpContext.Session.GetString("Account") is null ? Page() : RedirectToPage("/Products/Index");
    }

    public IActionResult OnPost()
    {
        if (string.IsNullOrWhiteSpace(AccountMember.UserName))
        {
            ModelState.AddModelError(string.Empty, "Username is required.");
            return Page();
        }

        var memberAccount = _accountService.GetAccountByLogin(AccountMember.UserName);
        if (memberAccount is not null && memberAccount.MemberPassword == AccountMember.MemberPassword && (memberAccount.MemberRole == 1 || memberAccount.MemberRole == 2))
        {
            HttpContext.Session.SetString("Account", memberAccount.MemberId.ToString());
            HttpContext.Session.SetString("Username", memberAccount.FullName);
            return RedirectToPage("/Products/Index");
        }

        ModelState.AddModelError(string.Empty, "You do not have permission to do this function!");
        return Page();
    }
}
