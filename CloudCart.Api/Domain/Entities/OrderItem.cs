namespace CloudCart.Api.Domain.Entities
{
    public class OrderItem
    {
        public int Id { get; set; }

        public int OrderId { get; set; }
        public Order? Order { get; set; }

        public int ProductId { get; set; }
        public Product? Product { get; set; }

        public int Quantity { get; set; }
        // Original product price at the time of purchase
        public decimal UnitPrice { get; set; }

        // Discount applied per unit
        public decimal DiscountAmount { get; set; }

        // Final price for this line
        public decimal TotalPrice { get; set; }
    }
}
