using Microsoft.AspNetCore.Http;
using PAW.Data;
using PAW.Models;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PAW.Services
{
    public class ProfileService : IProfileService
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;

        public ProfileService(ApplicationDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public string GetProfileImageUrl(string userId)
        {
            var photo = _context.Photos.FirstOrDefault(p => p.UserId == userId);
            return photo?.ImagePath ?? "/images/default-profile.jpg";
        }

        public async Task UploadProfileImageAsync(string userId, IFormFile image)
        {
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(image.FileName)}";
            var savePath = Path.Combine(_env.WebRootPath, "profile_images", fileName);

            Directory.CreateDirectory(Path.GetDirectoryName(savePath)!);

            using (var stream = new FileStream(savePath, FileMode.Create))
            {
                await image.CopyToAsync(stream);
            }

            var relativePath = $"/profile_images/{fileName}";

            var existing = _context.Photos.FirstOrDefault(p => p.UserId == userId);
            if (existing == null)
            {
                _context.Photos.Add(new Photo { UserId = userId, ImagePath = relativePath });
            }
            else
            {
                existing.ImagePath = relativePath;
                _context.Photos.Update(existing);
            }

            await _context.SaveChangesAsync();
        }
    }
}
