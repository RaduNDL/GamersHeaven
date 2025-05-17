using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using PAW.Models;
using PAW.Repositories;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System;

namespace PAW.Services
{
    public class GameService : IGameService
    {
        private readonly IGameRepository _repo;
        private readonly IWebHostEnvironment _env;

        public GameService(IGameRepository repo, IWebHostEnvironment env)
        {
            _repo = repo;
            _env = env;
        }

        public async Task<List<Game>> GetAllGamesAsync() =>
            await _repo.GetAllAsync();

        public async Task<Game> GetGameByIdAsync(int id) =>
            await _repo.GetByIdAsync(id);

        public async Task<List<Genre>> GetAllGenresAsync() =>
            await _repo.GetAllGenresAsync();

        public async Task<int> AddGenreIfNotExistsAsync(string name) =>
            await _repo.AddGenreIfNotExistsAsync(name);

        public async Task AddGameAsync(Game game, IFormFile imageFile)
        {
            game.ImageFileName = await SaveImageAsync(imageFile) ?? "default.jpg";
            await _repo.AddAsync(game);
        }

        public async Task UpdateGameAsync(Game updatedGame)
        {
            var existingGame = await _repo.GetByIdAsync(updatedGame.GameID);
            if (existingGame == null)
                return;

            existingGame.Price = updatedGame.Price;

            await _repo.UpdateAsync(existingGame);
        }

        public async Task DeleteGameAsync(int id)
        {
            var game = await _repo.GetByIdAsync(id);
            if (game == null)
                return;

            if (!string.IsNullOrWhiteSpace(game.ImageFileName) && game.ImageFileName != "default.jpg")
            {
                var imagePath = Path.Combine(_env.WebRootPath, "images", game.ImageFileName);
                if (File.Exists(imagePath))
                {
                    File.Delete(imagePath);
                }
            }

            await _repo.DeleteAsync(game);
        }

        private async Task<string> SaveImageAsync(IFormFile imageFile)
        {
            if (imageFile == null || imageFile.Length == 0)
                return null;

            var uploadsFolder = Path.Combine(_env.WebRootPath, "images");
            Directory.CreateDirectory(uploadsFolder);

            var uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(imageFile.FileName);
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(stream);
            }

            return uniqueFileName;
        }
    }
}
