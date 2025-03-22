using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.RegularExpressions;

namespace OnlineShop.Pages.Checkout
{
    public class CheckoutModel : PageModel
    {
        [BindProperty]
        public string PaymentMethod { get; set; }

        [BindProperty]
        public string CardNumber { get; set; }

        [BindProperty]
        public string ExpiryDate { get; set; }

        [BindProperty]
        public string CVV { get; set; }
        public IActionResult OnPost()
        {
            if (PaymentMethod == "Card")
            {
                // Validate Expiry Date (MM/YY format)
                if (!Regex.IsMatch(ExpiryDate, @"^(0[1-9]|1[0-2])\/\d{2}$"))
                {
                    TempData["ErrorMessage"] = "Invalid expiry date format. Use MM/YY.";
                    return RedirectToPage("Failure");
                }

                // Check if card is expired
                string[] parts = ExpiryDate.Split('/');
                int expMonth = int.Parse(parts[0]);
                int expYear = 2000 + int.Parse(parts[1]); // Assume 20XX format

                DateTime expiry = new DateTime(expYear, expMonth, DateTime.DaysInMonth(expYear, expMonth));
                if (expiry < DateTime.UtcNow)
                {
                    TempData["ErrorMessage"] = "Card has expired.";
                    return RedirectToPage("Failure");
                }

                // Simulate successful payment
                TempData["Message"] = "Payment successful! Thank you for your purchase.";
                return RedirectToPage("Success");
            }
            else if (PaymentMethod == "PayPal")
            {
                return Redirect("https://www.paypal.com/signin");
            }

            TempData["ErrorMessage"] = "Invalid payment method.";
            return RedirectToPage("Failure");
        }
    }
}
