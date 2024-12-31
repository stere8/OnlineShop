using Microsoft.AspNetCore.Mvc.RazorPages;
using OnlineShop.Models;
using OnlineShop.Services;

namespace OnlineShop.Pages.Categories
{
    public class IndexModel : PageModel
    {
        private readonly ICategoryService _categoryService;

        public IndexModel(ICategoryService categoryService)
        {
            _categoryService = categoryService ?? throw new ArgumentNullException(nameof(categoryService));
        }

        public IList<Category> Categories { get; set; }

        public async Task OnGetAsync()
        {
            Categories = (await _categoryService.GetAllCategoriesAsync()).ToList();
        }
    }
}