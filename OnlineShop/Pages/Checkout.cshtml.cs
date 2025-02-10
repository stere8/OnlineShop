using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OnlineShop.Services;
using System.Security.Claims;
using System.Threading.Tasks;

namespace OnlineShop.Pages
{
    public class CheckoutModel : PageModel
    {
        private readonly CheckoutService _checkoutService;

        public CheckoutModel(CheckoutService checkoutService)
        {
            _checkoutService = checkoutService;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return RedirectToPage("/Account/Login");

            try
            {
                var order = await _checkoutService.ProcessCheckout(userId);
                return RedirectToPage("/Orders/OrderConfirmation", new { orderId = order.OrderId });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return Page();  // Stay on the checkout page if there's an error
            }
        }
    }
}
