using Microsoft.EntityFrameworkCore;
using PAW.Data;
using PAW.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PAW.Repositories
{
    public class GameRepository : IGameRepository
    {
        private readonly ApplicationDbContext _context;

        public GameRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Game>> GetAllAsync() =>
            await _context.Games.Include(g => g.Genre).ToListAsync();

        public async Task<Game> GetByIdAsync(int id) =>
            await _context.Games.Include(g => g.Genre).FirstOrDefaultAsync(g => g.GameID == id);

        public async Task AddAsync(Game game)
        {
            _context.Games.Add(game);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Game game)
        {
            _context.Games.Update(game);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Game game)
        {
            _context.Games.Remove(game);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Genre>> GetAllGenresAsync() =>
            await _context.Genres.OrderBy(g => g.Name).ToListAsync();

        public async Task<int> AddGenreIfNotExistsAsync(string name)
        {
            var existing = await _context.Genres.FirstOrDefaultAsync(g => g.Name == name);
            if (existing != null) return existing.GenreId;

            var genre = new Genre { Name = name };
            _context.Genres.Add(genre);
            await _context.SaveChangesAsync();
            return genre.GenreId;
        }
    }
}
