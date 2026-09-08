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
    public class UserServiceTests
    {
        [Fact]
        public async Task CreateAsync_ValidDto_ReturnsCreatedUserDto()
        {
            // Arrange
            var repoMock = new Mock<IUserRepository>();
            repoMock.Setup(r => r.AddAsync(It.IsAny<User>()))
                .Callback<User>(u => u.Id = 7)
                .Returns(Task.CompletedTask);
            repoMock.Setup(r => r.SaveChangesAsync()).Returns(Task.CompletedTask);

            var svc = new UserService(repoMock.Object);
            var dto = new CreateUserDto { FullName = "Jane", Email = "j@e" };

            // Act
            var result = await svc.CreateAsync(dto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(7, result.Id);
            Assert.Equal(dto.FullName, result.FullName);
            Assert.Equal(dto.Email, result.Email);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsMappedList()
        {
            // Arrange
            var users = new List<User>
            {
                new User { Id = 1, FullName = "A", Email = "a@a" },
                new User { Id = 2, FullName = "B", Email = "b@b" }
            };
            var repoMock = new Mock<IUserRepository>();
            repoMock.Setup(r => r.ListAsync()).ReturnsAsync(users);

            var svc = new UserService(repoMock.Object);

            // Act
            var result = await svc.GetAllAsync();

            // Assert
            Assert.Collection(result,
                r => Assert.Equal("A", r.FullName),
                r => Assert.Equal("B", r.FullName));
        }

        [Fact]
        public async Task GetByIdAsync_UserNotFound_ReturnsNull()
        {
            // Arrange
            var repoMock = new Mock<IUserRepository>();
            repoMock.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((User)null);
            var svc = new UserService(repoMock.Object);

            // Act
            var result = await svc.GetByIdAsync(99);

            // Assert
            Assert.Null(result);
        }
    }
}
