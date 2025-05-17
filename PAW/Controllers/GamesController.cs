using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using System.Threading.Tasks;
using System.IO;
using System;
using PAW.Models;
using PAW.Services;
using System.Linq;

namespace PAW.Controllers
{
    [Authorize]
    public class GamesController : Controller
    {
        private readonly IGameService _gameService;
        private readonly IGenreService _genreService;
        private readonly IWebHostEnvironment _env;

        public GamesController(IGameService gameService, IGenreService genreService, IWebHostEnvironment env)
        {
            _gameService = gameService;
            _genreService = genreService;
            _env = env;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var games = await _gameService.GetAllGamesAsync();
            var genres = await _genreService.GetAllGenresAsync();
            ViewBag.Genres = genres;
            ViewBag.FavoriteGameIds = new List<int>(); 

            return View(games);
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var game = await _gameService.GetGameByIdAsync(id);
            if (game == null)
                return NotFound();

            ViewBag.CurrentUserId = User.Identity.IsAuthenticated ? User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value : "";
            return View(game);
        }

        [Authorize(Roles = "Administrator")]
        [HttpGet]
        public async Task<IActionResult> Add()
        {
            ViewBag.Genres = await _genreService.GetAllGenresAsync();
            return View();
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(Game game, string newGenre)
        {
            ModelState.Remove("newGenre");

            if (!string.IsNullOrWhiteSpace(newGenre))
            {
                var genre = await _genreService.GetOrCreateGenreAsync(newGenre.Trim());
                game.GenreId = genre.GenreId;
            }

            if (!ModelState.IsValid)
            {
                ViewBag.Genres = await _genreService.GetAllGenresAsync();
                return View(game);
            }

            if (game.ImageFile != null)
            {
                var uploadsFolder = Path.Combine(_env.WebRootPath, "images");
                Directory.CreateDirectory(uploadsFolder);
                var uniqueFileName = Guid.NewGuid() + "_" + Path.GetFileName(game.ImageFile.FileName);
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await game.ImageFile.CopyToAsync(stream);
                }

                game.ImageFileName = uniqueFileName;
            }

            await _gameService.AddGameAsync(game, game.ImageFile);
            TempData["SuccessMessage"] = "Game added successfully!";
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Administrator")]
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var game = await _gameService.GetGameByIdAsync(id);
            if (game == null)
                return NotFound();

            return View("EditGames", game);
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Game game)
        {
            if (!ModelState.IsValid)
                return View("EditGames", game);

            var existingGame = await _gameService.GetGameByIdAsync(game.GameID);
            if (existingGame == null)
                return NotFound();

            existingGame.Price = game.Price;

            await _gameService.UpdateGameAsync(existingGame);

            TempData["SuccessMessage"] = "Game price updated successfully!";
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Administrator")]
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var game = await _gameService.GetGameByIdAsync(id);
            if (game == null)
                return NotFound();

            return View("Delete", game);
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _gameService.DeleteGameAsync(id);
            TempData["SuccessMessage"] = "Game deleted successfully!";
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public async Task<IActionResult> AddGenreAjax([FromBody] string genreName)
        {
            if (string.IsNullOrWhiteSpace(genreName))
                return BadRequest("Genre name is required.");

            var genre = await _genreService.GetOrCreateGenreAsync(genreName.Trim());
            return Ok(new { id = genre.GenreId, name = genre.Name });
        }
    }
}
