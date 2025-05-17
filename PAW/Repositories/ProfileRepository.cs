using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using PAW.Data;
using PAW.Models;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PAW.Repositories
{
    public class ProfileRepository : IProfileRepository
    {
        private readonly ApplicationDbContext _context;

        public ProfileRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task SaveProfileImageAsync(string userId, IFormFile imageFile)
        {
            var fileName = $"{userId}_{Guid.NewGuid()}.jpg";
            var filePath = Path.Combine("wwwroot/profile_images", fileName);

            Directory.CreateDirectory("wwwroot/profile_images");

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(stream);
            }

            var relativePath = $"/profile_images/{fileName}";
            var existing = await _context.Photos.FirstOrDefaultAsync(p => p.UserId == userId);
            if (existing != null)
                existing.ImagePath = relativePath;
            else
                _context.Photos.Add(new Photo { UserId = userId, ImagePath = relativePath });

            await _context.SaveChangesAsync();
        }

        public string GetProfileImagePath(string userId)
        {
            var photo = _context.Photos.FirstOrDefault(p => p.UserId == userId);
            return photo?.ImagePath ?? "/images/default-profile.jpg";
        }
    }
}
