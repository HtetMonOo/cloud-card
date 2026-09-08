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
    public class ProductServiceTests
    {
        [Fact]
        public async Task CreateAsync_ValidDto_ReturnsCreatedProductDto()
        {
            // Arrange
            var repoMock = new Mock<IProductRepository>();
            repoMock.Setup(r => r.AddAsync(It.IsAny<Product>()))
                .Callback<Product>(p => p.Id = 42)
                .Returns(Task.CompletedTask);
            repoMock.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);

            var svc = new ProductService(repoMock.Object);
            var dto = new CreateProductDto { Name = "X", Price = 9.99m };

            // Act
            var result = await svc.CreateAsync(dto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(42, result.Id);
            Assert.Equal(dto.Name, result.Name);
            Assert.Equal(dto.Price, result.Price);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsMappedList()
        {
            // Arrange
            var products = new List<Product>
            {
                new Product { Id = 1, Name = "A", Price = 1m },
                new Product { Id = 2, Name = "B", Price = 2m }
            };
            var repoMock = new Mock<IProductRepository>();
            repoMock.Setup(r => r.ListAsync()).ReturnsAsync(products);

            var svc = new ProductService(repoMock.Object);

            // Act
            var result = await svc.GetAllAsync();

            // Assert
            Assert.Collection(result,
                r => Assert.Equal("A", r.Name),
                r => Assert.Equal("B", r.Name));
        }

        [Fact]
        public async Task GetByIdAsync_ProductNotFound_ReturnsNull()
        {
            // Arrange
            var repoMock = new Mock<IProductRepository>();
            repoMock.Setup(r => r.GetByIdWithDetailsAsync(It.IsAny<int>())).ReturnsAsync((Product)null);
            var svc = new ProductService(repoMock.Object);

            // Act
            var result = await svc.GetByIdAsync(123);

            // Assert
            Assert.Null(result);
        }
    }
}
