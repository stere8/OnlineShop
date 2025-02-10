using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Data;
using OnlineShop.Models;
using System.Security.Claims;
using System.Threading.Tasks;

namespace OnlineShop.Pages
{
    public class CartModel : PageModel
    {
        private readonly OnlineShopDbContext _context;

        public CartModel(OnlineShopDbContext context)
        {
            _context = context;
        }

        public List<CartItem> CartItems { get; set; } = new();
        public decimal TotalAmount { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                return RedirectToPage("/Account/LoginModel");
            }

            var cart = await _context.Carts
                .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Product)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null)
            {
                return Page();
            }

            CartItems = cart.CartItems.ToList();
            TotalAmount = CartItems.Sum(ci => ci.Quantity * ci.Product.Price);

            return Page();
        }

        public async Task<IActionResult> OnPostCheckoutAsync()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                return RedirectToPage("/Account/LoginModel");
            }

            var cart = await _context.Carts
                .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Product)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null || !cart.CartItems.Any())
            {
                return RedirectToPage("/Cart", new { message = "Your cart is empty." });
            }

            // Create the order from the cart items
            var order = new Order
            {
                UserId = userId,
                OrderDate = DateTime.UtcNow,
                Status = OrderStatus.Pending.ToString(),
                TotalAmount = cart.CartItems.Sum(ci => ci.Quantity * ci.Product.Price),
                OrderItems = cart.CartItems.Select(ci => new OrderItem
                {
                    ProductId = ci.ProductId,
                    Quantity = ci.Quantity,
                    Price = ci.Product.Price
                }).ToList()
            };

            // Deduct stock
            foreach (var cartItem in cart.CartItems)
            {
                var product = await _context.Products.FindAsync(cartItem.ProductId);
                if (product != null)
                {
                    if (product.StockQuantity < cartItem.Quantity)
                    {
                        TempData["Message"] = $"Insufficient stock for {product?.Name ?? "Product"}.";

                        // Redirect back to the Cart page
                        return RedirectToPage("/Cart");
                    }

                    product.StockQuantity -= cartItem.Quantity;
                }
            }

            _context.Orders.Add(order);
            _context.CartItems.RemoveRange(cart.CartItems);
            await _context.SaveChangesAsync();

            // Redirect to Order Confirmation
            return RedirectToPage("/Orders/OrderConfirmation", new { orderId = order.OrderId });
        }

        public async Task<IActionResult> OnPostUpdateQuantityAsync(int productId, int quantity)
        {
            if (quantity < 1) quantity = 1;

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return RedirectToPage("Account/Login"); //  Corrected path

            var cart = await _context.Carts
                .Include(c => c.CartItems)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart != null)
            {
                var cartItem = cart.CartItems.FirstOrDefault(ci => ci.ProductId == productId);
                if (cartItem != null)
                {
                    var product = await _context.Products.FindAsync(productId);
                    if (product != null && quantity <= product.StockQuantity)
                    {
                        cartItem.Quantity = quantity;
                        await _context.SaveChangesAsync();
                    }
                }
            }

            return RedirectToPage("Cart"); 
        }

        public async Task<IActionResult> OnPostRemoveItemAsync(int productId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return RedirectToPage("Account/Login"); //  Corrected path

            var cart = await _context.Carts
                .Include(c => c.CartItems)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart != null)
            {
                var cartItem = cart.CartItems.FirstOrDefault(ci => ci.ProductId == productId);
                if (cartItem != null)
                {
                    _context.CartItems.Remove(cartItem);
                    await _context.SaveChangesAsync();
                }
            }

            return RedirectToPage("Cart");
        }
    }
}
