using PAW.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PAW.Services
{
    public interface IGenreService
    {
        Task<List<Genre>> GetAllGenresAsync();
        Task<Genre> GetOrCreateGenreAsync(string name);
    }
}
