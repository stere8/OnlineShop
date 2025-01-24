using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OnlineShop.Models;
using OnlineShop.Services;

namespace OnlineShop.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly ICategoryService _categoryService;
        public IList<Category> Categories { get; set; }


        public IndexModel(ILogger<IndexModel> logger,ICategoryService categoryService)
        {
            _logger = logger;
            _categoryService = categoryService;
        }

        public async Task OnGetAsync()
        {
            var allCategories = await _categoryService.GetAllCategoriesAsync();

            Categories = allCategories
                .OrderBy(c => c.CategoryId == 8 ? 1 : 0) // This puts ID 8 last
                .ThenBy(c => c.Name)
                .ToList();
        }
    }
}
