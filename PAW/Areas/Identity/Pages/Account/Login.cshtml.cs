using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PAW.Services;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace PAW.Areas.Identity.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly IAccountService _accountService;

        public LoginModel(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public class InputModel
        {
            [Required]
            [Display(Name = "Email or Username")]
            public string Email { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Display(Name = "Remember me?")]
            public bool RememberMe { get; set; }
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl ?? Url.Content("~/");
            if (!ModelState.IsValid)
                return Page();

            var result = await _accountService.LoginAsync(Input.Email, Input.Password, Input.RememberMe);
            if (result.Succeeded)
                return LocalRedirect(ReturnUrl);

            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return Page();
        }
    }
}
