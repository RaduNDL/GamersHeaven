using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using PAW.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using PAW.Models;

namespace PAW.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IHomeService _homeService;

        public HomeController(IHomeService homeService)
        {
            _homeService = homeService;
        }

        public async Task<IActionResult> Index()
        {
            var topGames = await _homeService.GetTopGamesAsync();
            ViewBag.FavoriteGameIds = new List<int>();
            return View(topGames);
        }
    }
}
