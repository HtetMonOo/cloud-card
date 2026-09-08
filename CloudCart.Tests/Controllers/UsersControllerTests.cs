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
    public class UsersControllerTests
    {
        [Fact]
        public async Task Index_ReturnsViewWithList()
        {
            var svcMock = new Mock<IUserService>();
            svcMock.Setup(s => s.GetAllAsync()).ReturnsAsync(new List<UserDto>());
            var ctrl = new UsersController(svcMock.Object);

            var result = await ctrl.Index();

            var vr = Assert.IsType<ViewResult>(result);
            Assert.NotNull(vr.Model);
        }

        [Fact]
        public async Task Details_UserNotFound_ReturnsNotFound()
        {
            var svcMock = new Mock<IUserService>();
            svcMock.Setup(s => s.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((UserDto)null);
            var ctrl = new UsersController(svcMock.Object);

            var result = await ctrl.Details(1);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Create_Post_InvalidModel_ReturnsViewWithDto()
        {
            var svcMock = new Mock<IUserService>();
            var ctrl = new UsersController(svcMock.Object);
            ctrl.ModelState.AddModelError("Email", "required");
            var dto = new CreateUserDto { FullName = "", Email = "" };

            var result = await ctrl.Create(dto);

            var vr = Assert.IsType<ViewResult>(result);
            Assert.Equal(dto, vr.Model);
        }

        [Fact]
        public async Task Create_Post_Valid_RedirectsToDetails()
        {
            var svcMock = new Mock<IUserService>();
            svcMock.Setup(s => s.CreateAsync(It.IsAny<CreateUserDto>())).ReturnsAsync(new UserDto { Id = 5 });
            var ctrl = new UsersController(svcMock.Object);

            var result = await ctrl.Create(new CreateUserDto { FullName = "X", Email = "x@x" });

            var rr = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Details", rr.ActionName);
            Assert.Equal(5, rr.RouteValues["id"]);
        }
    }
}
