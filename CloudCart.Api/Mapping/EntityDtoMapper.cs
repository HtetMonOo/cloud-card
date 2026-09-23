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

        // User
        public static UserDto ToDto(this User u)
        {
            if (u == null) return null!;
            return new UserDto
            {
                Id = u.Id,
                FullName = u.FullName,
                Email = u.Email
            };
        }

        public static User ToEntity(this DTOs.CreateUserDto dto)
        {
            return new User
            {
                FullName = dto.FullName,
                Email = dto.Email
            };
        }

        // DiscountEvent
        public static DiscountEventDto ToDto(this DiscountEvent d)
        {
            return new DiscountEventDto
            {
                Id = d.Id,
                Name = d.Name,
                StartDate = d.StartDate,
                EndDate = d.EndDate,
                DiscountType = d.DiscountType,
                DiscountValue = d.DiscountValue,
                IsActive = d.IsActive,
                Products = d.Products
                    .Select(p => p.ToDto())
                    .ToList()
            };
        }

        public static DiscountEvent ToEntity(this DTOs.CreateDiscountEventDto dto)
        {
            return new DiscountEvent
            {
                Name = dto.Name,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                DiscountType = dto.DiscountType,
                DiscountValue = dto.DiscountValue
            };
        }

            


    }
}
