using CloudCart.Api.DTOs;
using CloudCart.Api.Domain.Entities;
using CloudCart.Api.Persistence.Repositories;
using CloudCart.Api.Services;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace CloudCart.Tests.Services
{
    public class OrderServiceTests
    {
        [Fact]
        public async Task CreateOrderAsync_ProductsExist_SetsUnitPricesAndReturnsOrderDto()
        {
            // Arrange
            var orderRepo = new Mock<IOrderRepository>();
            var productRepo = new Mock<IProductRepository>();

            productRepo.Setup(p => p.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((int id) => new Product { Id = id, Name = "P" + id, Price = id * 1.5m });

            orderRepo.Setup(r => r.AddAsync(It.IsAny<Order>()))
                .Callback<Order>(o => o.Id = 11)
                .Returns(Task.CompletedTask);
            orderRepo.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);

            orderRepo.Setup(r => r.GetByIdWithItemsAsync(It.IsAny<int>())).ReturnsAsync((int id) =>
            {
                var o = new Order { Id = id, UserId = 5 };
                o.Items.Add(new OrderItem { ProductId = 2, Quantity = 3, UnitPrice = 3.0m });
                return o;
            });

            var svc = new OrderService(orderRepo.Object, productRepo.Object);

            var dto = new CreateOrderDto { UserId = 5 };
            dto.Items.Add(new CreateOrderItemDto { ProductId = 2, Quantity = 3 });

            // Act
            var result = await svc.CreateOrderAsync(dto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(11, result.Id);
            Assert.Equal(1, result.Items.Count);
            Assert.Equal(3.0m, result.Items[0].UnitPrice);
        }

        [Fact]
        public async Task CreateOrderAsync_ProductMissing_ThrowsException()
        {
            // Arrange
            var orderRepo = new Mock<IOrderRepository>();
            var productRepo = new Mock<IProductRepository>();
            productRepo.Setup(p => p.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Product)null);

            var svc = new OrderService(orderRepo.Object, productRepo.Object);

            var dto = new CreateOrderDto { UserId = 5 };
            dto.Items.Add(new CreateOrderItemDto { ProductId = 99, Quantity = 1 });

            // Act & Assert
            await Assert.ThrowsAsync<System.Exception>(() => svc.CreateOrderAsync(dto));
        }

        [Fact]
        public async Task GetByIdAsync_OrderNotFound_ReturnsNull()
        {
            // Arrange
            var orderRepo = new Mock<IOrderRepository>();
            var productRepo = new Mock<IProductRepository>();
            orderRepo.Setup(r => r.GetByIdWithItemsAsync(It.IsAny<int>())).ReturnsAsync((Order)null);
            var svc = new OrderService(orderRepo.Object, productRepo.Object);

            // Act
            var result = await svc.GetByIdAsync(123);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetOrdersForUserAsync_ReturnsMappedList()
        {
            // Arrange
            var orderRepo = new Mock<IOrderRepository>();
            var productRepo = new Mock<IProductRepository>();
            orderRepo.Setup(r => r.GetOrdersForUserAsync(It.IsAny<int>())).ReturnsAsync(new List<Order>
            {
                new Order { Id = 1, UserId = 2 },
                new Order { Id = 2, UserId = 2 }
            });

            var svc = new OrderService(orderRepo.Object, productRepo.Object);

            // Act
            var result = await svc.GetOrdersForUserAsync(2);

            // Assert
            Assert.Equal(2, result.Count);
        }
    }
}
