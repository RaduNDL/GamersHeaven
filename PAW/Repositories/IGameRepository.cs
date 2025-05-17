using PAW.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PAW.Repositories
{
    public interface IGameRepository
    {
        Task<List<Game>> GetAllAsync();
        Task<Game> GetByIdAsync(int id);
        Task AddAsync(Game game);
        Task UpdateAsync(Game game);
        Task<List<Genre>> GetAllGenresAsync();
        Task<int> AddGenreIfNotExistsAsync(string name);
        Task DeleteAsync(Game game);

    }
}
    