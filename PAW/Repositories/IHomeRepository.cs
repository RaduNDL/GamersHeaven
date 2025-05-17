using PAW.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PAW.Repositories
{
    public interface IHomeRepository
    {
        Task<List<Game>> GetTopGamesAsync();
    }
}
