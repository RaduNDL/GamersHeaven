using Microsoft.AspNetCore.Mvc;
using PAW.Services;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PAW.Controllers
{
    public class FavoritesController : Controller
    {
        private readonly IFavoritesService _favoritesService;

        public FavoritesController(IFavoritesService favoritesService)
        {
            _favoritesService = favoritesService;
        }

        private string GetUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        public async Task<IActionResult> Index()
        {
            var userId = GetUserId();
            var favorites = await _favoritesService.GetOrCreateFavoritesAsync(userId);
            return View(favorites);
        }

        [HttpPost]
        public async Task<IActionResult> AddToFavorites(int gameId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var added = await _favoritesService.AddToFavoritesAsync(gameId, userId);
            return added ? Ok() : NotFound();
        }
        [HttpPost]
        public async Task<IActionResult> RemoveByGameId(int gameId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            await _favoritesService.RemoveFromFavoritesByGameIdAsync(gameId, userId);
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> RemoveByGameIdForm(int gameId)
        {
            var userId = GetUserId();
            await _favoritesService.RemoveFromFavoritesByGameIdAsync(gameId, userId);
            return RedirectToAction(nameof(Index));
        }

        public class FavoriteRequest
        {
            public int GameId { get; set; }
        }
    }
}
