using PAW.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PAW.Services
{
    public interface IReviewService
    {
        Task<List<object>> GetReviewsByGameWithPhotoAsync(int gameId);
        Task<List<Review>> GetAllReviewsAsync();
        Task<Review> GetReviewByIdAsync(int reviewId);
        Task AddReviewAsync(Review review);
        Task UpdateReviewAsync(Review review);
        Task DeleteReviewAsync(int reviewId);
    }
}
