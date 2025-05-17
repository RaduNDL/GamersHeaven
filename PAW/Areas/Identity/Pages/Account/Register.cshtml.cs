using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PAW.Services;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace PAW.Areas.Identity.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly IAccountService _accountService;

        public RegisterModel(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            [StringLength(100, MinimumLength = 6)]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Compare("Password", ErrorMessage = "Passwords do not match.")]
            public string ConfirmPassword { get; set; }
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl ?? Url.Content("~/");

            if (!ModelState.IsValid)
                return Page();

            var result = await _accountService.RegisterAsync(Input.Email, Input.Password);

            if (result.Succeeded)
                return RedirectToPage("RegisterConfirmation", new { email = Input.Email });

            foreach (var error in result.Errors)
                ModelState.AddModelError(string.Empty, error.Description);

            return Page();
        }
    }
}
