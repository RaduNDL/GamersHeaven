using PAW.Models;
using System.Threading.Tasks;

namespace PAW.Services
{
    public interface IFavoritesService
    {
        Task<FavoriteList> GetOrCreateFavoritesAsync(string userId);
        Task<bool> AddToFavoritesAsync(int gameId, string userId);
        Task RemoveFromFavoritesAsync(int favoriteItemId, string userId);
        Task RemoveFromFavoritesByGameIdAsync(int gameId, string userId);
    }
}
