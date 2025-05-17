using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PAW.Models;
using PAW.Services;
using System.Threading.Tasks;

namespace PAW.Controllers
{
    [Route("reviews")]
    public class ReviewsController : Controller
    {
        private readonly IReviewService _service;
        private readonly UserManager<IdentityUser> _userManager;

        public ReviewsController(IReviewService service, UserManager<IdentityUser> userManager)
        {
            _service = service;
            _userManager = userManager;
        }

        [HttpGet("{gameId}")]
        public async Task<IActionResult> GetReviews(int gameId)
        {
            var reviews = await _service.GetReviewsByGameWithPhotoAsync(gameId);
            return Json(reviews);
        }

        [Authorize]
        [HttpPost("add")]
        public async Task<IActionResult> AddReview([FromBody] Review review)
        {
            if (review.Rating < 1 || review.Rating > 5)
                return BadRequest("Rating must be between 1 and 5.");

            var userId = _userManager.GetUserId(User);
            review.UserId = userId;
            review.Date = DateTime.Now;
            await _service.AddReviewAsync(review);
            return Ok(review);
        }

        [Authorize]
        [HttpPut("edit/{id}")]
        public async Task<IActionResult> EditReview(int id, [FromBody] Review review)
        {
            if (id != review.ReviewID)
                return BadRequest("ID mismatch.");

            var reviewToUpdate = await _service.GetReviewByIdAsync(id);
            if (reviewToUpdate == null)
                return NotFound("Review not found.");

            var userId = _userManager.GetUserId(User);
            if (reviewToUpdate.UserId != userId)
                return Forbid();

            reviewToUpdate.Comment = review.Comment;
            reviewToUpdate.Rating = review.Rating;
            reviewToUpdate.Date = DateTime.Now;
            await _service.UpdateReviewAsync(reviewToUpdate);
            return Ok();
        }

        [Authorize]
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteReview(int id)
        {
            var review = await _service.GetReviewByIdAsync(id);
            if (review == null)
                return NotFound();

            var userId = _userManager.GetUserId(User);
            if (review.UserId != userId)
                return Forbid();

            await _service.DeleteReviewAsync(id);
            return Ok();
        }

        [HttpGet("/Reviews")]
        public async Task<IActionResult> Index()
        {
            var reviews = await _service.GetAllReviewsAsync();
            return View(reviews);
        }
    }
}
