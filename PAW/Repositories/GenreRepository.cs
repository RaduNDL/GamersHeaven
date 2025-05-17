using Microsoft.EntityFrameworkCore;
using PAW.Data;
using PAW.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PAW.Repositories
{
    public class GenreRepository : IGenreRepository
    {
        private readonly ApplicationDbContext _context;

        public GenreRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Genre>> GetAllAsync()
        {
            return await _context.Genres
                .OrderBy(g => g.Name)
                .ToListAsync();
        }

        public async Task<Genre> GetByNameAsync(string name)
        {
            return await _context.Genres
                .FirstOrDefaultAsync(g => g.Name.ToLower() == name.ToLower());
        }

        public async Task<Genre> GetOrCreateAsync(string name)
        {
            var existing = await GetByNameAsync(name);
            if (existing != null)
                return existing;

            var genre = new Genre { Name = name };
            _context.Genres.Add(genre);
            await _context.SaveChangesAsync();
            return genre;
        }

        public async Task AddAsync(Genre genre)
        {
            if (!string.IsNullOrWhiteSpace(genre.Name))
            {
                _context.Genres.Add(genre);
                await _context.SaveChangesAsync();
            }
        }

    }
}
