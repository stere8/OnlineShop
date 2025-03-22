using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Data;
using OnlineShop.Models;

namespace OnlineShop.Pages.Admin
{
    [Authorize(Roles = "Admin")]
    public class DashboardModel : PageModel
    {
        private readonly OnlineShopDbContext _context;

        public DashboardModel(OnlineShopDbContext context)
        {
            _context = context;
        }

        public int TotalUsers { get; set; }
        public int TotalOrders { get; set; }
        public int TotalProducts { get; set; }
        public decimal TotalRevenue { get; set; }
        public Dictionary<string, int> OrdersByStatus { get; set; } = new();
        public List<Product> LowStockProducts { get; set; } = new();


        public async Task OnGetAsync()
        {
            TotalUsers = await _context.Users.CountAsync();
            TotalProducts = await _context.Products.CountAsync();
            TotalOrders = await _context.Orders.CountAsync();
            TotalRevenue = await _context.Orders.SumAsync(o => o.TotalAmount);

            OrdersByStatus = await _context.Orders
                .GroupBy(o => o.Status)
                .Select(g => new { Status = g.Key, Count = g.Count() })
                .ToDictionaryAsync(g => g.Status, g => g.Count);

            // Low Stock Alerts
            LowStockProducts = await _context.Products
                .Where(p =>
                    _context.LowStockConfigs.Any(c => c.ProductId == p.ProductId && p.StockQuantity < c.MinThreshold))
                .ToListAsync();
        }
    }
}
