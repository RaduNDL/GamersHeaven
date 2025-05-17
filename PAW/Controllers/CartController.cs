using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PAW.Models;
using PAW.Services;

namespace PAW.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartService _cartService;
        private readonly UserManager<IdentityUser> _userManager;

        public CartController(ICartService cartService, UserManager<IdentityUser> userManager)
        {
            _cartService = cartService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var cart = await _cartService.GetOrCreateCartAsync();
            return View(cart);
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart([FromBody] AddToCartRequest request)
        {
            if (request == null || request.GameId == 0) return BadRequest();

            var success = await _cartService.AddToCartAsync(request.GameId);
            return success ? Ok(new { message = "Game added to cart!" }) : NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> RemoveFromCart(int cartItemId)
        {
            await _cartService.RemoveFromCartAsync(cartItemId);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateQuantity(int cartItemId, string action)
        {
            await _cartService.UpdateQuantityAsync(cartItemId, action);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Billing()
        {
            var cart = await _cartService.GetCartForBillingAsync();

            if (cart == null)
            {
                TempData["Message"] = "Cart is empty.";
                return RedirectToAction("Index");
            }

            return View("Billing", cart);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProcessPayment(string cardNumber, string nameOnCard, string expiryDate, string cvv)
        {
            if (string.IsNullOrWhiteSpace(cardNumber) ||
                string.IsNullOrWhiteSpace(nameOnCard) ||
                string.IsNullOrWhiteSpace(expiryDate) ||
                string.IsNullOrWhiteSpace(cvv))
            {
                TempData["Message"] = "Payment failed. Please fill out all required fields.";
                return RedirectToAction("Billing");
            }

            var userId = _userManager.GetUserId(User);
            var orderCreated = await _cartService.CheckoutAsync(userId);

            TempData["Message"] = orderCreated
                ? "✅ Payment successful! Thank you for your purchase."
                : "Something went wrong during payment. Please try again.";

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> GetItemCount()
        {
            var count = await _cartService.GetItemCountAsync();
            return Json(new { count });
        }

        public class AddToCartRequest
        {
            public int GameId { get; set; }
        }
    }
}
