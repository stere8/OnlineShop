using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OnlineShop.Models;
using OnlineShop.Services;

namespace OnlineShop.Pages.Categories
{
    [Authorize(Roles ="Admin")]
    public class AddEditCategoryModel : PageModel
    {

        private readonly ICategoryService _categoryService;
        private readonly IProductService _productService;

        [BindProperty]
        public Category Category { get; set; }

        public AddEditCategoryModel(ICategoryService categoryService,IProductService productService)
        {
            _categoryService = categoryService;
            _productService = productService;
        }

        public async Task<IActionResult> OnGetAsync(int? CategoryId)
        {
            if ( CategoryId == null )
            {
                Category = new Category();
                return Page();
            }

            Category = await _categoryService.GetCategoryByIdAsync(CategoryId.Value);


            if ( Category == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            if (Category.CategoryId == 0)
            {
                await _categoryService.AddCategoryAsync(Category);
            }
            else
            {
                await _categoryService.UpdateCategoryAsync(Category);
            }
            return RedirectToPage("/Categories/Index");
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            if (id == 8)
            {
                return RedirectToPage("/Categories/Index");
            }
            var productsToUpdate = await _productService.SearchProductsAsync("",id); // Assuming you have a method to get products by category
            foreach (var product in productsToUpdate)
            {
                product.CategoryId = 8; // Set the CategoryId to 0 (or another placeholder value)
                await _productService.UpdateProductAsync(product); // Update each product
            }

            await _categoryService.DeleteCategoryAsync(id);
            return RedirectToPage("/Categories/Index");
        }
    }
}
