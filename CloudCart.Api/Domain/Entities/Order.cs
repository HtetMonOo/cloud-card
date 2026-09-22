using System;
using System.Collections.Generic;

namespace CloudCart.Api.Domain.Entities
{
    public class Order
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public User? User { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public List<OrderItem> Items { get; set; } = new();

        public decimal Total => Items?.Sum(x => x.TotalPrice) ?? 0m;
    }
}
