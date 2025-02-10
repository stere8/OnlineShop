namespace OnlineShop.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public string UserId { get; set; }  // New: Link to user
        public DateTime OrderDate { get; set; }
        public string Status { get; set; } = "Pending";  // New: Track order status
        public ICollection<OrderItem> OrderItems { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
