using PAW.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PAW.Services
{
    public interface IHomeService
    {
        Task<List<Game>> GetTopGamesAsync();
    }
}
