using Microsoft.EntityFrameworkCore;
using PAW.Data;
using PAW.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PAW.Repositories
{
    public class HomeRepository : IHomeRepository
    {
        private readonly ApplicationDbContext _context;

        public HomeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Game>> GetTopGamesAsync()
        {
            return await _context.Games
                .Where(g => g.GameID == 6 || g.GameID == 4 || g.GameID == 8)
                .ToListAsync();
        }
    }
}
