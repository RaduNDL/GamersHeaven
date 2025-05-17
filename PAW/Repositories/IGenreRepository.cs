using PAW.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PAW.Repositories
{
    public interface IGenreRepository
    {
        Task<IEnumerable<Genre>> GetAllAsync();
        Task<Genre> GetByNameAsync(string name);
        Task<Genre> GetOrCreateAsync(string name);
        Task AddAsync(Genre genre);
    }
}
