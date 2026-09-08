using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Xunit;

using CloudCart.Api.Persistence;
using CloudCart.Api.Persistence.Repositories;
using CloudCart.Api.Services;
using CloudCart.Api.DTOs;

namespace CloudCart.Tests
{
    public class UserServiceTests
    {
        private AppDbContext CreateInMemoryDb(string name) => new AppDbContext(
            new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(name)
                .Options);

        [Fact]
        public async Task CreateAsync_AddsUser()
        {
            var dbName = Guid.NewGuid().ToString();
            using var db = CreateInMemoryDb(dbName);
            var repo = new UserRepository(db);
            var svc = new CloudCart.Api.Services.UserService(repo);

            var dto = new CreateUserDto { FullName = "John Doe", Email = "john@example.com" };
            var created = await svc.CreateAsync(dto);

            Assert.NotNull(created);
            Assert.Equal("John Doe", created.FullName);

            var all = await svc.GetAllAsync();
            Assert.Single(all);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsUser()
        {
            var dbName = Guid.NewGuid().ToString();
            using var db = CreateInMemoryDb(dbName);
            var repo = new UserRepository(db);
            var svc = new CloudCart.Api.Services.UserService(repo);

            var created = await svc.CreateAsync(new CreateUserDto { FullName = "Alice", Email = "alice@example.com" });

            var fetched = await svc.GetByIdAsync(created.Id);
            Assert.NotNull(fetched);
            Assert.Equal("Alice", fetched.FullName);
        }
    }
}
