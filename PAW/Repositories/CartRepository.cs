using Microsoft.EntityFrameworkCore;
using PAW.Data;
using PAW.Models;

namespace PAW.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly ApplicationDbContext _context;

        public CartRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ShoppingCart?> GetCartAsync()
        {
            return await _context.ShoppingCarts
                .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Game)
                .FirstOrDefaultAsync();
        }

        public async Task<ShoppingCart> GetOrCreateCartAsync()
        {
            var cart = await GetCartAsync();
            if (cart == null)
            {
                cart = new ShoppingCart { CartItems = new List<CartItem>() };
                _context.ShoppingCarts.Add(cart);
                await _context.SaveChangesAsync();
            }
            return cart;
        }

        public async Task<Game?> GetGameByIdAsync(int id)
        {
            return await _context.Games.FindAsync(id);
        }

        public async Task<CartItem?> GetCartItemByIdAsync(int id)
        {
            return await _context.CartItems.FindAsync(id);
        }

        public async Task AddCartItemAsync(CartItem item)
        {
            _context.CartItems.Add(item);
            await Task.CompletedTask;
        }

        public async Task RemoveCartItemAsync(CartItem item)
        {
            _context.CartItems.Remove(item);
            await Task.CompletedTask;
        }

        public async Task RemoveCartAsync(ShoppingCart cart)
        {
            _context.ShoppingCarts.Remove(cart);
            await Task.CompletedTask;
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
