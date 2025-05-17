using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace PAW.Repositories
{
    public interface IProfileRepository
    {
        Task SaveProfileImageAsync(string userId, IFormFile imageFile);
        string GetProfileImagePath(string userId);
    }
}
