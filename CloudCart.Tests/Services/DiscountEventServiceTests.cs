using CloudCart.Api.Domain.Entities;
using CloudCart.Api.DTOs;
using CloudCart.Api.Persistence.Repositories;
using CloudCart.Api.Persistence.Repositories.Interfaces;
using CloudCart.Api.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.Arm;
using System.Threading.Tasks;
using Xunit;

namespace CloudCart.Tests.Services
{
    public class DiscountEventServiceTests
    {
        [Fact]
        public async Task CreateDiscountEventAsync_ProductNotFound_Throws()
        {
            var discountRepo = new Mock<IDiscountEventRepository>();
            var productRepo = new Mock<IProductRepository>();

            var products = new List<Product>
            {
                new Product { Id = 1, Name = "P1", Price = 5m },
                new Product { Id = 2, Name = "P2", Price = 10m }
            };

            productRepo
                .Setup(p => p.GetProductsByIdsAsync(It.IsAny<List<int>>()))
                .ReturnsAsync((List<int> ids) =>
                    products.Where(p => ids.Contains(p.Id)).ToList());

            var svc = new DiscountEventService(discountRepo.Object, productRepo.Object);
            var dto = new CreateDiscountEventDto
            {
                Name = "Sale",
                StartDate = DateTime.UtcNow.AddDays(-1),
                EndDate = DateTime.UtcNow.AddDays(1),
                DiscountType = DiscountType.Percentage,
                DiscountValue = 10m,
                ProductIds = new List<int> { 1, 2, 3 }
            };

            await Assert.ThrowsAsync<Exception>(() => svc.CreateDiscountEventAsync(dto));

        }


        [Fact]
        public async Task CreateDiscountEventAsync_ValidDto_ReturnsCreatedDto()
        {
            // Arrange
            var discountRepo = new Mock<IDiscountEventRepository>();
            var productRepo = new Mock<IProductRepository>();

            var products = new List<Product>
            {
                new Product { Id = 1, Name = "P1", Price = 5m },
                new Product { Id = 2, Name = "P2", Price = 10m }
            };

            productRepo.Setup(p => p.GetProductsByIdsAsync(It.IsAny<List<int>>())).ReturnsAsync(products);

            discountRepo.Setup(r => r.AddAsync(It.IsAny<DiscountEvent>()))
                .Callback<DiscountEvent>(d => d.Id = 100)
                .Returns(Task.CompletedTask);
            discountRepo.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);
            discountRepo.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((int id) => new DiscountEvent
            {
                Id = id,
                Name = "Sale",
                StartDate = DateTime.UtcNow.AddDays(-1),
                EndDate = DateTime.UtcNow.AddDays(1),
                DiscountType = DiscountType.Percentage,
                DiscountValue = 10m,
                Products = products
            });

            var svc = new DiscountEventService(discountRepo.Object, productRepo.Object);

            var dto = new CreateDiscountEventDto
            {
                Name = "Sale",
                StartDate = DateTime.UtcNow.AddDays(-1),
                EndDate = DateTime.UtcNow.AddDays(1),
                DiscountType = DiscountType.Percentage,
                DiscountValue = 10m,
                ProductIds = new List<int> { 1, 2 }
            };

            // Act
            var result = await svc.CreateDiscountEventAsync(dto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(100, result.Id);
            Assert.Equal(2, result.Products.Count);
            Assert.Equal(dto.Name, result.Name);
        }

        [Fact]
        public async Task GetActiveDiscountsForProductAsync_ReturnsMappedList()
        {
            // Arrange
            var discountRepo = new Mock<IDiscountEventRepository>();
            var productRepo = new Mock<IProductRepository>();

            var de = new DiscountEvent { Id = 1, Name = "D", StartDate = DateTime.UtcNow.AddDays(-1), EndDate = DateTime.UtcNow.AddDays(1), DiscountType = DiscountType.FixedAmount, DiscountValue = 2m };
            discountRepo.Setup(r => r.GetActiveDiscountsForProductAsync(It.IsAny<int>())).ReturnsAsync(new List<DiscountEvent> { de });

            var svc = new DiscountEventService(discountRepo.Object, productRepo.Object);

            // Act
            var result = await svc.GetActiveDiscountsForProductAsync(1);

            // Assert
            Assert.Single(result);
            Assert.Equal(de.Name, result[0].Name);
        }

        [Fact]
        public async Task UpdateDiscountEventAsync_NotFound_Throws()
        {
            // Arrange
            var discountRepo = new Mock<IDiscountEventRepository>();
            var productRepo = new Mock<IProductRepository>();
            discountRepo.Setup(r => r.GetByIdWithProductsAsync(It.IsAny<int>())).ReturnsAsync((DiscountEvent)null);

            var svc = new DiscountEventService(discountRepo.Object, productRepo.Object);

            var dto = new UpdateDiscountEventDto { Name = "X", StartDate = DateTime.UtcNow, EndDate = DateTime.UtcNow.AddDays(1), DiscountType = DiscountType.FixedAmount, DiscountValue = 1m, ProductIds = new List<int> { 1 } };

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => svc.UpdateDiscountEventAsync(5, dto));
        }

        [Fact]
        public async Task UpdateDiscountEventAsync_ProductMissing_Throws()
        {
            // Arrange
            var discountRepo = new Mock<IDiscountEventRepository>();
            var productRepo = new Mock<IProductRepository>();

            var existing = new DiscountEvent { Id = 2, Name = "Old", Products = new List<Product>() };
            discountRepo.Setup(r => r.GetByIdWithProductsAsync(It.IsAny<int>())).ReturnsAsync(existing);

            // product repo returns fewer products than ids
            productRepo.Setup(p => p.GetProductsByIdsAsync(It.IsAny<List<int>>())).ReturnsAsync(new List<Product>());

            var svc = new DiscountEventService(discountRepo.Object, productRepo.Object);

            var dto = new UpdateDiscountEventDto { Name = "X", StartDate = DateTime.UtcNow, EndDate = DateTime.UtcNow.AddDays(1), DiscountType = DiscountType.FixedAmount, DiscountValue = 1m, ProductIds = new List<int> { 1, 2 } };

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => svc.UpdateDiscountEventAsync(2, dto));
        }

        [Fact]
        public async Task UpdateDiscountEventAsync_Valid_UpdatesAndReturnsDto()
        {
            // Arrange
            var discountRepo = new Mock<IDiscountEventRepository>();
            var productRepo = new Mock<IProductRepository>();

            var existing = new DiscountEvent { Id = 3, Name = "Old", Products = new List<Product>() };
            discountRepo.Setup(r => r.GetByIdWithProductsAsync(It.IsAny<int>())).ReturnsAsync(existing);

            var products = new List<Product> { new Product { Id = 7, Name = "P7" } };
            productRepo.Setup(p => p.GetProductsByIdsAsync(It.IsAny<List<int>>())).ReturnsAsync(products);

            discountRepo.Setup(r => r.Update(It.IsAny<DiscountEvent>()));
            discountRepo.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);

            var svc = new DiscountEventService(discountRepo.Object, productRepo.Object);

            var dto = new UpdateDiscountEventDto { Name = "New", StartDate = DateTime.UtcNow, EndDate = DateTime.UtcNow.AddDays(2), DiscountType = DiscountType.FixedAmount, DiscountValue = 5m, ProductIds = new List<int> { 7 } };

            // Act
            var result = await svc.UpdateDiscountEventAsync(3, dto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("New", result.Name);
            Assert.Single(result.Products);
            Assert.Equal(7, result.Products[0].Id);
        }
    }
}
