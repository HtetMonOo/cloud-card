using System.Collections.Generic;

namespace CloudCart.Api.Domain.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public decimal Price { get; set; }

        public List<OrderItem> OrderItems { get; set; } = new();
    }
}
