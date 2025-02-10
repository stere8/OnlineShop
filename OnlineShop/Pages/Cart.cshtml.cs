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
                return RedirectToPage("Account/Login"); 

            var cart = await _context.Carts
                .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Product)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart != null)
            {
                CartItems = cart.CartItems.ToList();
                TotalAmount = cart.CartItems.Sum(ci => ci.Product.Price * ci.Quantity);
            }

            return Page();
        }

        public async Task<IActionResult> OnPostUpdateQuantityAsync(int productId, int quantity)
        {
            if (quantity < 1) quantity = 1;

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return RedirectToPage("Account/Login"); // ?? Corrected path

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
                return RedirectToPage("Account/Login"); // ?? Corrected path

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
