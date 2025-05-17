using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace PAW.Services
{
    public interface IAccountService
    {
        Task<SignInResult> LoginAsync(string email, string password, bool rememberMe);
        Task<IdentityResult> RegisterAsync(string email, string password);
        Task LogoutAsync();
    }
}
