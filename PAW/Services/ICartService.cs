using PAW.Models;

namespace PAW.Services
{
    public interface ICartService
    {
        Task<ShoppingCart> GetOrCreateCartAsync();
        Task<bool> AddToCartAsync(int gameId);
        Task RemoveFromCartAsync(int cartItemId);
        Task UpdateQuantityAsync(int cartItemId, string action);
        Task<ShoppingCart?> GetCartForBillingAsync();
        Task<bool> ProcessPaymentAsync();
        Task<int> GetItemCountAsync();

        Task<bool> CheckoutAsync(string userId);
    }
}
