using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Xunit;

using CloudCart.Api.Persistence;
using CloudCart.Api.Persistence.Repositories;
using CloudCart.Api.Services;
using CloudCart.Api.DTOs;

namespace CloudCart.Tests
{
    public class OrderServiceTests
    {
        private AppDbContext CreateInMemoryDb(string name) => new AppDbContext(
            new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(name)
                .Options);

        [Fact]
        public async Task CreateOrderAsync_SetsUnitPricesAndSaves()
        {
            var dbName = Guid.NewGuid().ToString();
            using var db = CreateInMemoryDb(dbName);

            // Seed product
            var product = new CloudCart.Api.Domain.Entities.Product { Name = "P1", Price = 5m };
            await db.Products.AddAsync(product);
            await db.SaveChangesAsync();

            var orderRepo = new OrderRepository(db);
            var productRepo = new ProductRepository(db);
            var svc = new OrderService(orderRepo, productRepo);

            var dto = new CreateOrderDto { UserId = 1, Items = new System.Collections.Generic.List<CreateOrderItemDto> { new CreateOrderItemDto { ProductId = product.Id, Quantity = 2 } } };

            var created = await svc.CreateOrderAsync(dto);

            Assert.NotNull(created);
            Assert.Equal(10m, created.Total);
            Assert.Single(created.Items);
            Assert.Equal(5m, created.Items.First().UnitPrice);
        }

        [Fact]
        public async Task GetOrdersForUserAsync_ReturnsOrders()
        {
            var dbName = Guid.NewGuid().ToString();
            using var db = CreateInMemoryDb(dbName);

            // Seed data: user, product, order
            var product = new CloudCart.Api.Domain.Entities.Product { Name = "P2", Price = 3m };
            await db.Products.AddAsync(product);
            var user = new CloudCart.Api.Domain.Entities.User { FullName = "U1", Email = "u1@example.com" };
            await db.Users.AddAsync(user);
            await db.SaveChangesAsync();

            var order = new CloudCart.Api.Domain.Entities.Order { UserId = user.Id };
            order.Items.Add(new CloudCart.Api.Domain.Entities.OrderItem { ProductId = product.Id, Quantity = 1, UnitPrice = product.Price });
            await db.Orders.AddAsync(order);
            await db.SaveChangesAsync();

            var orderRepo = new OrderRepository(db);
            var productRepo = new ProductRepository(db);
            var svc = new OrderService(orderRepo, productRepo);

            var list = await svc.GetOrdersForUserAsync(user.Id);
            Assert.Single(list);
            Assert.Equal(user.Id, list.First().UserId);
        }
    }
}
