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

        public decimal Total => Items == null ? 0m : CalculateTotal();

        private decimal CalculateTotal()
        {
            decimal sum = 0;
            foreach (var it in Items)
            {
                sum += it.UnitPrice * it.Quantity;
            }
            return sum;
        }
    }
}
