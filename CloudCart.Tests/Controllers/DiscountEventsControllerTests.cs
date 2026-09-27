using CloudCart.Api.Controllers;
using CloudCart.Api.DTOs;
using CloudCart.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using CloudCart.Api.Domain.Entities;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace CloudCart.Tests.Controllers
{
    public class DiscountEventsControllerTests
    {
        [Fact]
        public async Task Index_ReturnsViewWithList()
        {
            var svcMock = new Mock<IDiscountEventService>();
            svcMock.Setup(s => s.GetActiveDiscountsAsync()).ReturnsAsync(new List<DiscountEventDto>());
            var productMock = new Mock<CloudCart.Api.Services.Interfaces.IProductService>();
            var ctrl = new DiscountEventsController(svcMock.Object, productMock.Object);

            var result = await ctrl.Index();

            var vr = Assert.IsType<ViewResult>(result);
            Assert.NotNull(vr.Model);
        }

        [Fact]
        public async Task Details_NotFound_ReturnsNotFound()
        {
            var svcMock = new Mock<IDiscountEventService>();
            svcMock.Setup(s => s.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((DiscountEventDto)null);
            var productMock = new Mock<CloudCart.Api.Services.Interfaces.IProductService>();
            var ctrl = new DiscountEventsController(svcMock.Object, productMock.Object);

            var result = await ctrl.Details(1);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Create_Get_PopulatesProducts()
        {
            var svcMock = new Mock<IDiscountEventService>();
            var productMock = new Mock<CloudCart.Api.Services.Interfaces.IProductService>();
            productMock.Setup(p => p.GetAllAsync()).ReturnsAsync(new List<ProductDto> { new ProductDto { Id = 1, Name = "P1" } });

            var ctrl = new DiscountEventsController(svcMock.Object, productMock.Object);

            var result = await ctrl.Create();

            var vr = Assert.IsType<ViewResult>(result);
            var products = ctrl.ViewBag.Products as List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>;
            Assert.NotNull(products);
            Assert.Single(products);
        }

        [Fact]
        public async Task Create_Post_InvalidModel_ReturnsViewWithDto()
        {
            var svcMock = new Mock<IDiscountEventService>();
            var productMock = new Mock<CloudCart.Api.Services.Interfaces.IProductService>();
            productMock.Setup(p => p.GetAllAsync()).ReturnsAsync(new List<ProductDto>());
            var ctrl = new DiscountEventsController(svcMock.Object, productMock.Object);
            ctrl.ModelState.AddModelError("Name", "required");

            var dto = new CreateDiscountEventDto();

            var result = await ctrl.Create(dto);

            var vr = Assert.IsType<ViewResult>(result);
            Assert.Equal(dto, vr.Model);
        }

        [Fact]
        public async Task Create_Post_Valid_RedirectsToDetails()
        {
            var svcMock = new Mock<IDiscountEventService>();
            var productMock = new Mock<CloudCart.Api.Services.Interfaces.IProductService>();
            svcMock.Setup(s => s.CreateDiscountEventAsync(It.IsAny<CreateDiscountEventDto>())).ReturnsAsync(new DiscountEventDto { Id = 55 });

            var ctrl = new DiscountEventsController(svcMock.Object, productMock.Object);

            var dto = new CreateDiscountEventDto { Name = "X" };

            var result = await ctrl.Create(dto);

            var rr = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Details", rr.ActionName);
            Assert.Equal(55, rr.RouteValues["id"]);
        }

        [Fact]
        public async Task Edit_Get_NotFound_ReturnsNotFound()
        {
            var svcMock = new Mock<IDiscountEventService>();
            var productMock = new Mock<CloudCart.Api.Services.Interfaces.IProductService>();
            svcMock.Setup(s => s.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((DiscountEventDto)null);
            var ctrl = new DiscountEventsController(svcMock.Object, productMock.Object);

            var result = await ctrl.Edit(1);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Edit_Get_PopulatesModelAndProducts()
        {
            var svcMock = new Mock<IDiscountEventService>();
            var productMock = new Mock<CloudCart.Api.Services.Interfaces.IProductService>();

            var dto = new DiscountEventDto { Id = 2, Name = "D", StartDate = System.DateTime.UtcNow, EndDate = System.DateTime.UtcNow.AddDays(1), DiscountType = DiscountType.Percentage, DiscountValue = 5m, Products = new List<ProductDto> { new ProductDto { Id = 3 } } };
            svcMock.Setup(s => s.GetByIdAsync(2)).ReturnsAsync(dto);
            productMock.Setup(p => p.GetAllAsync()).ReturnsAsync(new List<ProductDto> { new ProductDto { Id = 3, Name = "P3" } });

            var ctrl = new DiscountEventsController(svcMock.Object, productMock.Object);

            var result = await ctrl.Edit(2);

            var vr = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<UpdateDiscountEventDto>(vr.Model);
            Assert.Equal("D", model.Name);
            var products = ctrl.ViewBag.Products as List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>;
            Assert.NotNull(products);
            Assert.Single(products);
        }

        [Fact]
        public async Task Edit_Post_InvalidModel_ReturnsView()
        {
            var svcMock = new Mock<IDiscountEventService>();
            var productMock = new Mock<CloudCart.Api.Services.Interfaces.IProductService>();
            productMock.Setup(p => p.GetAllAsync()).ReturnsAsync(new List<ProductDto>());
            var ctrl = new DiscountEventsController(svcMock.Object, productMock.Object);
            ctrl.ModelState.AddModelError("Name", "required");

            var dto = new UpdateDiscountEventDto();

            var result = await ctrl.Edit(1, dto);

            var vr = Assert.IsType<ViewResult>(result);
            Assert.Equal(dto, vr.Model);
        }

        [Fact]
        public async Task Edit_Post_Valid_RedirectsToDetails()
        {
            var svcMock = new Mock<IDiscountEventService>();
            var productMock = new Mock<CloudCart.Api.Services.Interfaces.IProductService>();
            svcMock.Setup(s => s.UpdateDiscountEventAsync(It.IsAny<int>(), It.IsAny<UpdateDiscountEventDto>())).ReturnsAsync(new DiscountEventDto { Id = 9 });

            var ctrl = new DiscountEventsController(svcMock.Object, productMock.Object);

            var dto = new UpdateDiscountEventDto { Name = "A" };
            var result = await ctrl.Edit(9, dto);

            var rr = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Details", rr.ActionName);
            Assert.Equal(9, rr.RouteValues["id"]);
        }
    }
}
