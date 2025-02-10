using OnlineShop.Data;
using OnlineShop.Models;
using Microsoft.EntityFrameworkCore;

namespace OnlineShop.Services
{
    public class CartService : ICartService
    {
        private readonly OnlineShopDbContext _context;

        public CartService(OnlineShopDbContext context)
        {
            _context = context;
        }

        public async Task<Cart> GetCartByUserAsync(string userId)
        {
            return await _context.Carts
                .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Product)
                .FirstOrDefaultAsync(c => c.UserId == userId);
        }

        public async Task<Order> ConvertCartToOrder(Cart cart, string userId)
        {
            var order = new Order
            {
                UserId = userId,
                OrderDate = DateTime.UtcNow,
                Status = "Pending",
                OrderItems = cart.CartItems.Select(ci => new OrderItem
                {
                    ProductId = ci.ProductId,
                    Quantity = ci.Quantity,
                    Price = ci.Product.Price
                }).ToList(),
                TotalAmount = cart.CartItems.Sum(ci => ci.Product.Price * ci.Quantity)
            };

            cart.Status = CartStatus.Paid;
            await ClearCartAsync(cart.CartId);

            return order;
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
                    throw new Exception("Cart is empty.");

                var existingOrder = await _context.Orders
                    .AnyAsync(o => o.UserId == userId && o.Status == "Pending");

                if (existingOrder)
                    throw new Exception("You already have a pending order.");

                // Create order
                var order = new Order
                {
                    UserId = userId,
                    OrderDate = DateTime.UtcNow,
                    Status = "Pending",
                    TotalAmount = cart.CartItems.Sum(ci => ci.Quantity * ci.Product.Price),
                    OrderItems = cart.CartItems.Select(ci => new OrderItem
                    {
                        ProductId = ci.ProductId,
                        Quantity = ci.Quantity,
                        Price = ci.Product.Price
                    }).ToList()
                };

                _context.Orders.Add(order);

                // Deduct product stock
                foreach (var cartItem in cart.CartItems)
                {
                    var product = await _context.Products.FindAsync(cartItem.ProductId);
                    if (product != null)
                    {
                        if (product.StockQuantity < cartItem.Quantity)
                            throw new Exception($"Not enough stock for {product.Name}.");

                        product.StockQuantity -= cartItem.Quantity; 
                    }
                }

                // Remove cart items after checkout
                _context.CartItems.RemoveRange(cart.CartItems);

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

        public async Task ClearCartAsync(int cartId)
        {
            var cartItems = await _context.CartItems
                .Where(ci => ci.CartId == cartId)
                .ToListAsync();

            _context.CartItems.RemoveRange(cartItems);
            await _context.SaveChangesAsync();
        }

        public async Task<ICollection<CartItem>> GetCartItemsAsync(string userId)
        {
            var cart = await GetCartByUserAsync(userId);
            return cart?.CartItems ?? new List<CartItem>();
        }
    }
}