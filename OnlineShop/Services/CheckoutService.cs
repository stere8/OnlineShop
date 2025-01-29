using OnlineShop.Data;
using OnlineShop.Models;

namespace OnlineShop.Services
{
    public class CheckoutService
    {
        private readonly OnlineShopDbContext _context;
        private CartService _cartService;

        public CheckoutService(OnlineShopDbContext context, CartService cartService)
        {
            _context = context;
            _cartService = cartService;
        }

        public async Task<Order> ProcessCheckout(Cart cart)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                //Converting cart to order
                var order = await _cartService.ConvertCartToOrder(cart);

                //Saving
                _context.Orders.Add(order);

                _context.Carts.Update(cart);

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
