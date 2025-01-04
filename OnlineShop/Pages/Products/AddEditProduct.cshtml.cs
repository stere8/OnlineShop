using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using OnlineShop.Models;
using OnlineShop.Services;

namespace OnlineShop.Pages.Products
{
    public class AddEditProductModel : PageModel
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;

        [BindProperty]
        public Product Product { get; set; }
        public SelectList Categories { get; set; }
        public int? ProductId { get; set; }

        public AddEditProductModel(IProductService productService, ICategoryService categoryService)
        {
            _productService = productService;
            _categoryService = categoryService;
        }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            ProductId = id;

            var categories = await _categoryService.GetAllCategoriesAsync();
            Categories = new SelectList(categories, "CategoryId", "Name");

            if (id.HasValue)
            {
                Product = await _productService.GetProductByIdAsync(id.Value);
                if (Product == null)
                {
                    return NotFound();
                }
            }
            else
            {
                Product = new Product();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(IFormFile ImageFile)
        {
            if (!ModelState.IsValid)
            {
                var categories = await _categoryService.GetAllCategoriesAsync();
                Categories = new SelectList(categories, "CategoryId", "Name");
                return Page();
            }

            if (Product.ProductId == 0)
            {
                if (ImageFile != null)
                {
                    Product.ImageUrl = await SaveImageAsync(ImageFile);
                }
                await _productService.AddProductAsync(Product);
            }
            else
            {
                if (ImageFile != null)
                {
                    Product.ImageUrl = await SaveImageAsync(ImageFile);
                }
                await _productService.UpdateProductAsync(Product);
            }

            return RedirectToPage("./Index");
        }

        public async Task<IActionResult> OnPostDeleteAsync()
        {
            if (Product.ProductId != 0)
            {
                await _productService.DeleteProductAsync(Product.ProductId);
            }
            return RedirectToPage("./Index");
        }

        private async Task<string> SaveImageAsync(IFormFile imageFile)
        {
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
            Directory.CreateDirectory(uploadsFolder); // Ensure the folder exists

            var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(fileStream);
            }

            return uniqueFileName; // Return the relative path for display
        }
    }
}
