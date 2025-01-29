using OnlineShop.Data;
using OnlineShop.Models;

namespace OnlineShop.Services
{
    public class CartService
    {
        public async Task<Order> ConvertCartToOrder(Cart cart)
        {
            // Create new order
            var order = new Order
            {
                OrderDate = DateTime.UtcNow,
                OrderItems = cart.CartItems.Select(ci => new OrderItem
                {
                    ProductId = ci.ProductId,
                    Quantity = ci.Quantity,
                    Price = ci.Product.Price
                }).ToList(),
                TotalAmount = cart.CartItems.Sum(ci => ci.Product.Price * ci.Quantity)
            };

            // Update cart status
            cart.Status = CartStatus.Paid;

            return order;
        }
    }
}