using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Data;
using OnlineShop.Models;
using OnlineShop.Services;
using System.Security.AccessControl;

namespace OnlineShop.Pages.Products
{
    public class IndexModel : PageModel
    {
        private readonly IProductService _productService;

        public IndexModel(IProductService productService)
        {
            _productService = productService ?? throw new ArgumentNullException(nameof(productService));
        }
        public IList<Product> Products { get; set; }
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }
        public async Task OnGetAsync(int pageNumber = 1, int pageSize = 6)
        {
            var allProductsQuery = await _productService.GetAllProductsQueryable();

            var (paginatedProducts, totalCount) = await _productService.GetPaginatedProductsAsync(pageNumber, pageSize, allProductsQuery);
            Products = paginatedProducts.ToList();
            CurrentPage = pageNumber;
            TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
        }
    }
}