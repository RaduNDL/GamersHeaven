using PAW.Models;

namespace PAW.Repositories
{
    public interface IFavoritesRepository
    {
        Task<FavoriteList?> GetFavoritesAsync(string userId);
        Task<FavoriteList> GetOrCreateFavoritesAsync(string userId);
        Task<Game?> GetGameByIdAsync(int gameId);
        Task AddItemAsync(FavoriteItem item);
        Task<FavoriteItem?> GetItemByIdAsync(int id);
        Task RemoveItemAsync(FavoriteItem item);
        Task RemoveFavoritesAsync(FavoriteList favorites);
        Task SaveAsync();
    }
}
