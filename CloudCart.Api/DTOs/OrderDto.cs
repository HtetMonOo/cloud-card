using System;
using System.Collections.Generic;

namespace CloudCart.Api.DTOs
{
    public class OrderDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<OrderItemDto> Items { get; set; } = new();
        public decimal Total { get; set; }
    }
}
