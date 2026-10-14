
using CloudCart.IntegrationTests.Infrastructure;
using Xunit;

namespace CloudCart.IntegrationTests.Controllers;

public class HomeControllerTests
    : IClassFixture<CloudCartWebApplicationFactory>
{
    private readonly HttpClient _client;

    public HomeControllerTests(
        CloudCartWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task HomePage_ReturnsSuccessStatusCode()
    {
        // Act
        var response = await _client.GetAsync("/");

        // Assert
        response.EnsureSuccessStatusCode();
    }
}
