using PAW.Models;
using PAW.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PAW.Services
{
    public class GenreService : IGenreService
    {
        private readonly IGenreRepository _repo;

        public GenreService(IGenreRepository repo)
        {
            _repo = repo;
        }

        public async Task<List<Genre>> GetAllGenresAsync()
        {
            var genres = await _repo.GetAllAsync();
            return new List<Genre>(genres);
        }

        public async Task<Genre> GetOrCreateGenreAsync(string name)
        {
            return await _repo.GetOrCreateAsync(name);
        }
    }
}
