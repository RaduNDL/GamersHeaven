using PAW.Models;

namespace PAW.Repositories
{
    public interface ICartRepository
    {
        Task<ShoppingCart?> GetCartAsync();
        Task<ShoppingCart> GetOrCreateCartAsync();
        Task<Game?> GetGameByIdAsync(int id);
        Task<CartItem?> GetCartItemByIdAsync(int id);
        Task AddCartItemAsync(CartItem item);
        Task RemoveCartItemAsync(CartItem item);
        Task RemoveCartAsync(ShoppingCart cart);
        Task SaveAsync();
    }
}
