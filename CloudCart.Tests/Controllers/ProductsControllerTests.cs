using CloudCart.Api.Controllers;
using CloudCart.Api.DTOs;
using CloudCart.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace CloudCart.Tests.Controllers
{
    public class ProductsControllerTests
    {
        [Fact]
        public async Task Index_ReturnsViewWithList()
        {
            // Arrange
            var svcMock = new Mock<IProductService>();
            svcMock.Setup(s => s.GetAllAsync()).ReturnsAsync(new List<ProductDto>());
            var ctrl = new ProductsController(svcMock.Object);

            // Act
            var result = await ctrl.Index();

            // Assert
            var vr = Assert.IsType<ViewResult>(result);
            Assert.NotNull(vr.Model);
        }

        [Fact]
        public async Task Details_ProductNotFound_ReturnsNotFound()
        {
            // Arrange
            var svcMock = new Mock<IProductService>();
            svcMock.Setup(s => s.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((ProductDto)null);
            var ctrl = new ProductsController(svcMock.Object);

            // Act
            var result = await ctrl.Details(5);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Create_Post_InvalidModel_ReturnsViewWithDto()
        {
            // Arrange
            var svcMock = new Mock<IProductService>();
            var ctrl = new ProductsController(svcMock.Object);
            ctrl.ModelState.AddModelError("Name", "required");
            var dto = new CreateProductDto { Name = "", Price = 1m };

            // Act
            var result = await ctrl.Create(dto);

            // Assert
            var vr = Assert.IsType<ViewResult>(result);
            Assert.Equal(dto, vr.Model);
        }

        [Fact]
        public async Task Create_Post_Valid_RedirectsToDetails()
        {
            // Arrange
            var svcMock = new Mock<IProductService>();
            svcMock.Setup(s => s.CreateAsync(It.IsAny<CreateProductDto>())).ReturnsAsync(new ProductDto { Id = 99 });
            var ctrl = new ProductsController(svcMock.Object);

            // Act
            var result = await ctrl.Create(new CreateProductDto { Name = "X", Price = 1m });

            // Assert
            var rr = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Details", rr.ActionName);
            Assert.Equal(99, rr.RouteValues["id"]);
        }
    }
}
