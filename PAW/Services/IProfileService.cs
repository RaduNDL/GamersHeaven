using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace PAW.Services
{
    public interface IProfileService
    {
        string GetProfileImageUrl(string userId);
        Task UploadProfileImageAsync(string userId, IFormFile image);
    }
}
