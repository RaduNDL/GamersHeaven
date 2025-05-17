using PAW.Data;
using PAW.Models;
using PAW.Repositories;

namespace PAW.Services
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _repository;
        private readonly ApplicationDbContext _context; // Asigură-te că îl injectezi!

        public CartService(ICartRepository repository, ApplicationDbContext context)
        {
            _repository = repository;
            _context = context;
        }

        public async Task<ShoppingCart> GetOrCreateCartAsync()
        {
            return await _repository.GetOrCreateCartAsync();
        }

        public async Task<bool> AddToCartAsync(int gameId)
        {
            var game = await _repository.GetGameByIdAsync(gameId);
            if (game == null) return false;

            var cart = await _repository.GetOrCreateCartAsync();

            var existingItem = cart.CartItems.FirstOrDefault(ci => ci.GameID == gameId);
            if (existingItem != null)
            {
                existingItem.Quantity++;
            }
            else
            {
                cart.CartItems.Add(new CartItem
                {
                    GameID = game.GameID,
                    Quantity = 1,
                    Price = game.Price, // FOLOSEȘTE DOAR PRICE
                    ShoppingCartID = cart.CartID
                });
            }

            await _repository.SaveAsync();
            return true;
        }

        public async Task RemoveFromCartAsync(int cartItemId)
        {
            var item = await _repository.GetCartItemByIdAsync(cartItemId);
            if (item == null) return;

            var cart = await _repository.GetCartAsync();
            if (cart == null) return;

            await _repository.RemoveCartItemAsync(item);
            await _repository.SaveAsync();

            if (!cart.CartItems.Any())
            {
                await _repository.RemoveCartAsync(cart);
                await _repository.SaveAsync();
            }
        }

        public async Task UpdateQuantityAsync(int cartItemId, string action)
        {
            var item = await _repository.GetCartItemByIdAsync(cartItemId);
            if (item == null) return;

            if (action == "increase")
                item.Quantity++;
            else if (action == "decrease" && item.Quantity > 1)
                item.Quantity--;
            else if (action == "decrease")
                await _repository.RemoveCartItemAsync(item);

            await _repository.SaveAsync();

            var cart = await _repository.GetCartAsync();
            if (cart != null && !cart.CartItems.Any())
            {
                await _repository.RemoveCartAsync(cart);
                await _repository.SaveAsync();
            }
        }

        public async Task<ShoppingCart?> GetCartForBillingAsync()
        {
            var cart = await _repository.GetCartAsync();
            return cart?.CartItems.Any() == true ? cart : null;
        }

        public async Task<bool> ProcessPaymentAsync()
        {
            // Logica mutată în CheckoutAsync
            return true;
        }

        public async Task<int> GetItemCountAsync()
        {
            var cart = await _repository.GetCartAsync();
            return cart?.CartItems.Sum(i => i.Quantity) ?? 0;
        }

        // Checkout: CREEAZĂ Order și OrderItems folosind Price
        public async Task<bool> CheckoutAsync(string userId)
        {
            var cart = await _repository.GetCartAsync();
            if (cart == null || !cart.CartItems.Any())
                return false;

            var order = new Orders
            {
                OrderDate = DateTime.UtcNow,
                TotalAmount = cart.CartItems.Sum(i => i.Quantity * i.Price),
                UserId = userId,
                OrderItems = cart.CartItems.Select(ci => new OrderItem
                {
                    GameID = ci.GameID,
                    Quantity = ci.Quantity,
                    Price = ci.Price 
                }).ToList()
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            // Golește coșul
            foreach (var item in cart.CartItems.ToList())
                await _repository.RemoveCartItemAsync(item);
            await _repository.RemoveCartAsync(cart);
            await _repository.SaveAsync();

            return true;
        }
    }
}
