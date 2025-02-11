using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Data;
using OnlineShop.Models;
using OnlineShop.Services;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace OnlineShop.Pages
{
    public class WishlistModel : PageModel
    {
        private readonly WishlistService _wishlistService;
        private readonly OnlineShopDbContext _context;

        public WishlistModel(WishlistService wishlistService, OnlineShopDbContext context)
        {
            _wishlistService = wishlistService;
            _context = context;
        }

        public IList<Product> Products { get; set; }
        public ICollection<WishlistItem> WishlistItems { get; set; }

        public async Task OnGetAsync()
        {
            var userId = User.FindFirstValue(System.Security.Claims.ClaimTypes.NameIdentifier);
            var wishlist = await _wishlistService.GetWishlistByUserAsync(userId);

            WishlistItems = wishlist?.WishlistItems ?? new List<WishlistItem>();
            Products = await _context.Products.ToListAsync();
        }

        public async Task<IActionResult> OnPostAddToWishlistAsync(int productId)
        {
            var userId = User.FindFirstValue(System.Security.Claims.ClaimTypes.NameIdentifier);
            await _wishlistService.AddItemToWishlistAsync(userId, productId);
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostRemoveFromWishlistAsync(int wishlistItemId)
        {
            await _wishlistService.RemoveItemFromWishlistAsync(wishlistItemId);
            return RedirectToPage();
        }
    }
}
