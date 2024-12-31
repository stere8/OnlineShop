using Microsoft.AspNetCore.Mvc.RazorPages;
using OnlineShop.Models;
using OnlineShop.Services;
using Microsoft.AspNetCore.Mvc;

namespace OnlineShop.Pages.Products
{
    public class ViewModel : PageModel
    {
        private readonly IProductService _productService;

        public ViewModel(IProductService productService)
        {
            _productService = productService;
        }

        public Product Product { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Product = await _productService.GetProductByIdAsync(id);

            if (Product == null)
            {
                return NotFound();
            }

            return Page();
        }
    }
}
