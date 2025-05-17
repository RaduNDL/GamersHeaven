using Microsoft.EntityFrameworkCore;
using PAW.Data;
using PAW.Models;

namespace PAW.Repositories
{
    public class FavoritesRepository : IFavoritesRepository
    {
        private readonly ApplicationDbContext _context;

        public FavoritesRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<FavoriteList?> GetFavoritesAsync(string userId)
        {
            return await _context.FavoriteLists
                .Include(f => f.FavoriteItems)
                .ThenInclude(fi => fi.Game)
                .FirstOrDefaultAsync(f => f.FavoriteItems.Any());
        }

        public async Task<FavoriteList> GetOrCreateFavoritesAsync(string userId)
        {
            var favoriteList = await GetFavoritesAsync(userId);

            if (favoriteList == null)
            {
                favoriteList = new FavoriteList();
                _context.FavoriteLists.Add(favoriteList);
                await _context.SaveChangesAsync();
            }

            return favoriteList;
        }

        public async Task<Game?> GetGameByIdAsync(int gameId)
        {
            return await _context.Games.FirstOrDefaultAsync(g => g.GameID == gameId);
        }

        public async Task AddItemAsync(FavoriteItem item)
        {
            _context.FavoriteItems.Add(item);
            await _context.SaveChangesAsync();
        }

        public async Task<FavoriteItem?> GetItemByIdAsync(int id)
        {
            return await _context.FavoriteItems.FindAsync(id);
        }

        public async Task RemoveItemAsync(FavoriteItem item)
        {
            _context.FavoriteItems.Remove(item);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveFavoritesAsync(FavoriteList favorites)
        {
            _context.FavoriteLists.Remove(favorites);
            await _context.SaveChangesAsync();
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
