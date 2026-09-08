using CloudCart.Api.DTOs;
using CloudCart.Api.Mapping;
using CloudCart.Api.Persistence.Repositories;
using CloudCart.Api.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CloudCart.Api.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;

        public OrderService(IOrderRepository orderRepository, IProductRepository productRepository)
        {
            _orderRepository = orderRepository;
            _productRepository = productRepository;
        }

        public async Task<OrderDto> CreateOrderAsync(CreateOrderDto dto)
        {
            // Build entity and set unit prices from products
            var order = dto.ToEntity();

            foreach (var item in order.Items)
            {
                var product = await _productRepository.GetByIdAsync(item.ProductId);
                if (product == null)
                {
                    // For simplicity, throw. Controller can catch and return 400.
                    throw new System.Exception($"Product {item.ProductId} not found");
                }
                item.UnitPrice = product.Price;
            }

            await _orderRepository.AddAsync(order);
            await _orderRepository.SaveChangesAsync();

            var created = await _orderRepository.GetByIdWithItemsAsync(order.Id);
            return created!.ToDto();
        }

        public async Task<OrderDto?> GetByIdAsync(int id)
        {
            var order = await _orderRepository.GetByIdWithItemsAsync(id);
            return order?.ToDto();
        }

        public async Task<List<OrderDto>> GetOrdersForUserAsync(int userId)
        {
            var list = await _orderRepository.GetOrdersForUserAsync(userId);
            return list.Select(o => o.ToDto()).ToList();
        }
    }
}
