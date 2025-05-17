using Microsoft.EntityFrameworkCore;
using PAW.Data;
using PAW.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PAW.Repositories
{
    public class ReviewsRepository : IReviewsRepository 
    { 
        private readonly ApplicationDbContext _context;
        public ReviewsRepository(ApplicationDbContext context) => _context = context;
         
        public async Task<List<object>> GetByGameIdWithPhotoAsync(int gameId)
        {
            return await _context.Reviews
                .Where(r => r.GameID == gameId)
                .OrderByDescending(r => r.Date)
                .Select(r => new
                {
                    r.ReviewID,
                    r.Rating,
                    r.Comment,
                    r.Date,
                    r.UserId,
                    UserName = _context.Users.Where(u => u.Id == r.UserId).Select(u => u.UserName).FirstOrDefault(),
                    ImagePath = _context.Photos.Where(p => p.UserId == r.UserId).Select(p => p.ImagePath).FirstOrDefault() ?? "/images/default_avatar.jpg"
                })
                .ToListAsync<object>();
        }

        public async Task<List<Review>> GetAllAsync()
            => await _context.Reviews.Include(r => r.Game)
                                     .OrderByDescending(r => r.Date)
                                     .ToListAsync();

        public async Task<Review> GetByIdAsync(int reviewId)
            => await _context.Reviews.FindAsync(reviewId);

        public async Task AddAsync(Review review)
            => await _context.Reviews.AddAsync(review);

        public async Task UpdateAsync(Review review)
            => _context.Reviews.Update(review);

        public async Task DeleteAsync(Review review)
            => _context.Reviews.Remove(review);

        public async Task SaveAsync()
            => await _context.SaveChangesAsync();
    }
}
