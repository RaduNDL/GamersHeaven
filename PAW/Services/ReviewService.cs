using PAW.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PAW.Services
{
    public class ReviewService : IReviewService
    {
        private readonly IReviewsRepository _repo;
        public ReviewService(IReviewsRepository repo) => _repo = repo;

        public async Task<List<object>> GetReviewsByGameWithPhotoAsync(int gameId)
            => await _repo.GetByGameIdWithPhotoAsync(gameId);

        public async Task<List<PAW.Models.Review>> GetAllReviewsAsync()
            => await _repo.GetAllAsync();

        public async Task<PAW.Models.Review> GetReviewByIdAsync(int reviewId)
            => await _repo.GetByIdAsync(reviewId);

        public async Task AddReviewAsync(PAW.Models.Review review)
        {
            await _repo.AddAsync(review);
            await _repo.SaveAsync();
        }

        public async Task UpdateReviewAsync(PAW.Models.Review review)
        {
            await _repo.UpdateAsync(review);
            await _repo.SaveAsync();
        }

        public async Task DeleteReviewAsync(int reviewId)
        {
            var review = await _repo.GetByIdAsync(reviewId);
            if (review != null)
            {
                await _repo.DeleteAsync(review);
                await _repo.SaveAsync();
            }
        }
    }
}
