using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Data;
using OnlineShop.Models;
using System.Security.Claims;

namespace OnlineShop.Services
{
    public class CheckoutService
    {
        private readonly OnlineShopDbContext _context;
        private readonly CartService _cartService;
        private readonly UserManager<ApplicationUser> _userManager;

        public CheckoutService(OnlineShopDbContext context, CartService cartService, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _cartService = cartService;
            _userManager = userManager;
        }

        public async Task<Order> ProcessCheckout(string userId)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var cart = await _context.Carts
                    .Include(c => c.CartItems)
                    .ThenInclude(ci => ci.Product)
                    .FirstOrDefaultAsync(c => c.UserId == userId);

                if (cart == null || !cart.CartItems.Any())
                    throw new Exception("Cart is empty");

                // Convert Cart to Order
                var order = await _cartService.ConvertCartToOrder(cart, userId);

                // Save Order and Update Cart
                _context.Orders.Add(order);
                _context.Carts.Remove(cart);  // Remove cart after checkout

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return order;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}
