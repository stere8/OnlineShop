namespace OnlineShop.Models
{
    public class Order
    {
        public int OrderId { get; set; } // This should be your primary key
        public DateTime OrderDate { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; } // Navigation property
        public decimal TotalAmount { get; set; }
    }
}
