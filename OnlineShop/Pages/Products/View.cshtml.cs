using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Data;
using OnlineShop.Models;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShop.Pages.Products
{
    public class ViewModel : PageModel
    {
        private readonly OnlineShopDbContext _context;

        public ViewModel(OnlineShopDbContext context)
        {
            _context = context;
        }

        public Product Product { get; set; }
        public List<Product> RelatedProducts { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Product = await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.ProductId == id);

            if (Product == null)
            {
                return NotFound();
            }

            // Fetch related products (same category, but different product)
            RelatedProducts = await _context.Products
                .Where(p => p.CategoryId == Product.CategoryId && p.ProductId != id)
                .Take(4)
                .ToListAsync();

            return Page();
        }

        public async Task<IActionResult> OnPostAddToCartWithQuantityAsync(int productId, int quantity)
        {
            if (quantity < 1) quantity = 1;  // Prevent invalid input

            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToPage("/Account/Login");
            }

            var product = await _context.Products.FindAsync(productId);
            if (product == null || product.StockQuantity < quantity)
            {
                return RedirectToPage("/Cart", new { message = "Product unavailable or insufficient stock" });
            }

            var cart = await _context.Carts
                .Include(c => c.CartItems)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null)
            {
                cart = new Cart { UserId = userId, CartItems = new List<CartItem>() };
                _context.Carts.Add(cart);
            }
                
            if (cart.CartItems == null)
            {
                cart.CartItems = new List<CartItem>();
            }

            var cartItem = cart.CartItems.FirstOrDefault(ci => ci.ProductId == productId);
            if (cartItem == null)
            {
                cartItem = new CartItem { ProductId = productId, Quantity = quantity };
                cart.CartItems.Add(cartItem);
            }
            else
            {
                cartItem.Quantity += quantity;
            }

            await _context.SaveChangesAsync();
            return RedirectToPage("Cart");
        }
    }
}
