using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace PAW.Services
{
    public class AccountService : IAccountService
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;

        public AccountService(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        public async Task<SignInResult> LoginAsync(string emailOrUsername, string password, bool rememberMe)
        {
            IdentityUser user = await _userManager.FindByNameAsync(emailOrUsername);
            if (user == null)
                user = await _userManager.FindByEmailAsync(emailOrUsername);

            if (user == null)
                return SignInResult.Failed;

            // Autentifică-te DOAR cu UserName!
            return await _signInManager.PasswordSignInAsync(user.UserName, password, rememberMe, lockoutOnFailure: false);
        }

        public async Task<IdentityResult> RegisterAsync(string email, string password)
        {
            var user = new IdentityUser { UserName = email, Email = email };
            return await _userManager.CreateAsync(user, password);
        }

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }
    }
}
