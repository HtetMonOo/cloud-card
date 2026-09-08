using CloudCart.Api.Domain.Entities;
using CloudCart.Api.DTOs;
using System.Linq;

namespace CloudCart.Api.Mapping
{
    public static class EntityDtoMapper
    {
        // Product
        public static ProductDto ToDto(this Product p)
        {
            if (p == null) return null!;
            return new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price
            };
        }

        public static Product ToEntity(this DTOs.CreateProductDto dto)
        {
            return new Product
            {
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price
            };
        }

        // Order
        public static OrderDto ToDto(this Order o)
        {
            if (o == null) return null!;
            var dto = new OrderDto
            {
                Id = o.Id,
                UserId = o.UserId,
                CreatedAt = o.CreatedAt,
                Items = o.Items?.Select(i => new OrderItemDto
                {
                    ProductId = i.ProductId,
                    ProductName = i.Product?.Name,
                    Quantity = i.Quantity,
                    UnitPrice = i.UnitPrice
                }).ToList() ?? new System.Collections.Generic.List<OrderItemDto>(),
                Total = o.Total
            };
            return dto;
        }

        public static Order ToEntity(this DTOs.CreateOrderDto dto)
        {
            var order = new Order
            {
                UserId = dto.UserId,
            };

            foreach (var it in dto.Items)
            {
                order.Items.Add(new OrderItem
                {
                    ProductId = it.ProductId,
                    Quantity = it.Quantity,
                    UnitPrice = 0m
                });
            }

            return order;
        }
    }
}
