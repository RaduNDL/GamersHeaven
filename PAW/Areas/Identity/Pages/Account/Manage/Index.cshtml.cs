using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace PAW.Areas.Identity.Pages.Account.Manage
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public IndexModel(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [Display(Name = "Username")]
            public string Username { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "New password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "Passwords do not match.")]
            public string ConfirmPassword { get; set; }
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");

            Input = new InputModel
            {
                Username = user.UserName
            };

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");

            if (!ModelState.IsValid)
                return Page();

            if (user.UserName != Input.Username)
            {
                var setUsernameResult = await _userManager.SetUserNameAsync(user, Input.Username);
                if (!setUsernameResult.Succeeded)
                {
                    foreach (var error in setUsernameResult.Errors)
                        ModelState.AddModelError(string.Empty, error.Description);
                    return Page();
                }
            }

            if (!string.IsNullOrWhiteSpace(Input.Password))
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var resetPassResult = await _userManager.ResetPasswordAsync(user, token, Input.Password);
                if (!resetPassResult.Succeeded)
                {
                    foreach (var error in resetPassResult.Errors)
                        ModelState.AddModelError(string.Empty, error.Description);
                    return Page();
                }
            }

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated!";
            return RedirectToPage();
        }
    }
}
