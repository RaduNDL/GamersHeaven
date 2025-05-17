using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;
using PAW.Services;

namespace PAW.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly IProfileService _profileService;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public ProfileController(
            IProfileService profileService,
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager)
        {
            _profileService = profileService;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var identityUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var imageUrl = _profileService.GetProfileImageUrl(identityUserId)
                           ?? "/images/default-profile.jpg";

            var user = await _userManager.FindByIdAsync(identityUserId);
            string userName = user?.UserName ?? "User";

            ViewBag.UserId = identityUserId;
            ViewBag.ProfileImage = imageUrl;
            ViewBag.UserName = userName;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(IFormFile profileImage, string UserName)
        {
            var identityUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByIdAsync(identityUserId);

            bool valid = true;
            string newProfileImageUrl = null;
            string error = null;
            bool usernameChanged = false;

            // Upload profile image dacă există fișier
            if (profileImage != null && profileImage.Length > 0)
            {
                await _profileService.UploadProfileImageAsync(identityUserId, profileImage);
                newProfileImageUrl = _profileService.GetProfileImageUrl(identityUserId) ?? "/images/default-profile.jpg";
            }

            // Schimbare username cu validare de unicitate
            if (!string.IsNullOrWhiteSpace(UserName))
            {
                if (UserName.Length >= 3 && UserName.Length <= 32)
                {
                    var existingUser = await _userManager.FindByNameAsync(UserName);
                    if (existingUser != null && existingUser.Id != identityUserId)
                    {
                        valid = false;
                        error = "Username already exists!";
                    }
                    else if (user != null && user.UserName != UserName)
                    {
                        user.UserName = UserName;
                        var result = await _userManager.UpdateAsync(user);
                        if (!result.Succeeded)
                        {
                            valid = false;
                            error = result.Errors.FirstOrDefault()?.Description ?? "Update failed!";
                        }
                        else
                        {
                            usernameChanged = true;
                        }
                    }
                }
                else
                {
                    valid = false;
                    error = "Username must be between 3 and 32 characters!";
                }
            }

            // REAUTENTIFICARE dacă ai schimbat username-ul (ca să se schimbe peste tot, permanent!)
            if (usernameChanged)
            {
                await _signInManager.RefreshSignInAsync(user);
            }

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return Json(new
                {
                    success = valid,
                    userName = user?.UserName,
                    profileImageUrl = newProfileImageUrl,
                    error = error
                });
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
