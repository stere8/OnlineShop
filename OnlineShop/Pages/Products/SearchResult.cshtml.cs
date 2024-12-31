using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OnlineShop.Models;
using OnlineShop.Services;

namespace OnlineShop.Pages.Products
{
    public class SearchResultModel : PageModel
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;

        public SearchResultModel(IProductService productService, ICategoryService categoryService)
        {
            _productService = productService;
            _categoryService = categoryService;
        }
        public string Query { get; set; }
        public IList<Category> Categories { get; set; } = new List<Category>();
        public IList<Product> Products { get; set; } = new List<Product>();
        public int? SelectedCategoryId { get; set; }
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }

        public async Task OnGetAsync(string? query, int? categoryId, int pageNumber = 1, int pageSize = 2)
        {
            Query = query;
            SelectedCategoryId = categoryId;

            Categories = (await _categoryService.GetAllCategoriesAsync()).ToList();

            var searchResults = await _productService.SearchProductsAsync(Query, SelectedCategoryId);

            var (products, totalCount) = await _productService.GetPaginatedProductsAsync(pageNumber, pageSize, searchResults);

            Products = products.ToList();
            CurrentPage = pageNumber;
            TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
        }

    }
}
