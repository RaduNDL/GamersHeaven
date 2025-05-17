using Microsoft.AspNetCore.Http;
using PAW.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface IGameService
{
    Task<List<Game>> GetAllGamesAsync();
    Task<Game> GetGameByIdAsync(int id);
    Task AddGameAsync(Game game, IFormFile imageFile);
    Task<List<Genre>> GetAllGenresAsync();
    Task<int> AddGenreIfNotExistsAsync(string name);
    Task UpdateGameAsync(Game game);
    Task DeleteGameAsync(int id);
}

