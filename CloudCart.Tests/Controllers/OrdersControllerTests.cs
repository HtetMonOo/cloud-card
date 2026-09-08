using CloudCart.Api.Controllers;
using CloudCart.Api.DTOs;
using CloudCart.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace CloudCart.Tests.Controllers
{
    public class OrdersControllerTests
    {
        [Fact]
        public async Task Create_Get_ReturnsViewWithProducts()
        {
            var orderMock = new Mock<IOrderService>();
            var productMock = new Mock<IProductService>();
            productMock.Setup(p => p.GetAllAsync()).ReturnsAsync(new List<ProductDto>
            {
                new ProductDto { Id = 1, Name = "P1" }
            });

            var ctrl = new OrdersController(orderMock.Object, productMock.Object);

            var result = await ctrl.Create();

            var vr = Assert.IsType<ViewResult>(result);
            // ViewBag populated from product list
            var products = ctrl.ViewBag.Products as List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>;
            Assert.NotNull(products);
            Assert.Single(products);
        }

        [Fact]
        public async Task Create_Post_InvalidModel_ReturnsViewWithDto()
        {
            var orderMock = new Mock<IOrderService>();
            var productMock = new Mock<IProductService>();
            productMock.Setup(p => p.GetAllAsync()).ReturnsAsync(new List<ProductDto>());

            var ctrl = new OrdersController(orderMock.Object, productMock.Object);
            // invalid: no items
            var dto = new CreateOrderDto { UserId = 1 };

            var result = await ctrl.Create(dto);

            var vr = Assert.IsType<ViewResult>(result);
            Assert.Equal(dto, vr.Model);
        }

        [Fact]
        public async Task Create_Post_Valid_RedirectsToDetails()
        {
            var orderMock = new Mock<IOrderService>();
            var productMock = new Mock<IProductService>();

            var created = new OrderDto { Id = 77 };
            orderMock.Setup(o => o.CreateOrderAsync(It.IsAny<CreateOrderDto>())).ReturnsAsync(created);
            var ctrl = new OrdersController(orderMock.Object, productMock.Object);

            var dto = new CreateOrderDto { UserId = 1 };
            dto.Items.Add(new CreateOrderItemDto { ProductId = 1, Quantity = 1 });

            var result = await ctrl.Create(dto);

            var rr = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Details", rr.ActionName);
            Assert.Equal(77, rr.RouteValues["id"]);
        }

        [Fact]
        public async Task Details_OrderNotFound_ReturnsNotFound()
        {
            var orderMock = new Mock<IOrderService>();
            var productMock = new Mock<IProductService>();
            orderMock.Setup(o => o.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((OrderDto)null);
            var ctrl = new OrdersController(orderMock.Object, productMock.Object);

            var result = await ctrl.Details(1);

            Assert.IsType<NotFoundResult>(result);
        }
    }
}
