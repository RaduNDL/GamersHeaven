using PAW.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PAW.Repositories
{
    public interface IReviewsRepository
    {
        Task<List<object>> GetByGameIdWithPhotoAsync(int gameId);
        Task<List<Review>> GetAllAsync();
        Task<Review> GetByIdAsync(int reviewId);
        Task AddAsync(Review review); 
        Task UpdateAsync(Review review);
        Task DeleteAsync(Review review);
        Task SaveAsync();
    }
}
