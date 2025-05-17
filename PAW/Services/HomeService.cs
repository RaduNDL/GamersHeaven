using PAW.Models;
using PAW.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PAW.Services
{
    public class HomeService : IHomeService
    {
        private readonly IHomeRepository _homeRepository;

        public HomeService(IHomeRepository homeRepository)
        {
            _homeRepository = homeRepository;
        }

        public async Task<List<Game>> GetTopGamesAsync()
        {
            return await _homeRepository.GetTopGamesAsync();
        }
    }
}
