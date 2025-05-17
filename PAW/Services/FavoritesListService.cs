using PAW.Models;
using PAW.Repositories;
using System.Linq;
using System.Threading.Tasks;

namespace PAW.Services
{
    public class FavoritesListService : IFavoritesService
    {
        private readonly IFavoritesRepository _repository;

        public FavoritesListService(IFavoritesRepository repository)
        {
            _repository = repository;
        }

        public async Task<FavoriteList> GetOrCreateFavoritesAsync(string userId)
        {
            return await _repository.GetOrCreateFavoritesAsync(userId);
        }

        public async Task<bool> AddToFavoritesAsync(int gameId, string userId)
        {
            var game = await _repository.GetGameByIdAsync(gameId);
            if (game == null) return false;

            var favorites = await _repository.GetOrCreateFavoritesAsync(userId);

            if (!favorites.FavoriteItems.Any(i => i.GameID == game.GameID))
            {
                var item = new FavoriteItem
                {
                    GameID = game.GameID,
                    GameTitle = game.Title,
                    FavoriteListID = favorites.FavoriteListID
                };

                await _repository.AddItemAsync(item);
            }

            return true;
        }

        public async Task RemoveFromFavoritesAsync(int favoriteItemId, string userId)
        {
            var item = await _repository.GetItemByIdAsync(favoriteItemId);
            if (item != null)
            {
                var favorites = await _repository.GetFavoritesAsync(userId);
                await _repository.RemoveItemAsync(item);

                if (favorites != null)
                {
                    favorites.FavoriteItems.Remove(item);

                    if (!favorites.FavoriteItems.Any())
                    {
                        await _repository.RemoveFavoritesAsync(favorites);
                    }
                }
            }
        }

        public async Task RemoveFromFavoritesByGameIdAsync(int gameId, string userId)
        {
            var favorites = await _repository.GetFavoritesAsync(userId);
            if (favorites == null) return;

            var item = favorites.FavoriteItems.FirstOrDefault(i => i.GameID == gameId);
            if (item != null)
            {
                await _repository.RemoveItemAsync(item);
                favorites.FavoriteItems.Remove(item);

                if (!favorites.FavoriteItems.Any())
                {
                    await _repository.RemoveFavoritesAsync(favorites);
                }
            }
        }
    }
}
