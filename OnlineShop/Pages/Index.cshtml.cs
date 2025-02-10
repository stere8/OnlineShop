using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Data;
using OnlineShop.Models;
using OnlineShop.Services;
using System.Security.Claims;

public class IndexModel : PageModel
{
    private readonly OnlineShopDbContext _context;
    private readonly ICategoryService _categoryService;

    public IndexModel(OnlineShopDbContext context, ICategoryService categoryService)
    {
        _context = context;
        _categoryService = categoryService;
    }

    public IList<Category> Categories { get; set; }
    public int CartItemCount { get; set; }

    public async Task OnGetAsync()
    {
        // Check if the user is authenticated
        if (User.Identity.IsAuthenticated)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var cart = await _context.Carts
                .Include(c => c.CartItems)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            // Set the CartItemCount in ViewData to be accessed in the layout
            CartItemCount = cart?.CartItems?.Sum(ci => ci.Quantity) ?? 0;
            ViewData["CartItemCount"] = CartItemCount;
        }

        // Get categories (existing logic)
        var allCategories = await _categoryService.GetAllCategoriesAsync();
        Categories = allCategories
            .OrderBy(c => c.CategoryId == 8 ? 1 : 0) // Sort CategoryId = 8 last
            .ThenBy(c => c.Name)
            .ToList();
    }
}
