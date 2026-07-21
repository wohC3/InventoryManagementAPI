using InventoryManagement.Controllers;
using InventoryManagement.Interfaces;
using Moq;
using Microsoft.AspNetCore.Mvc;
using InventoryManagement.Models;
using Microsoft.Extensions.Configuration;


namespace API.Tests.Controllers;

public class AuthControllerTest
{
    private readonly Mock<IAuthService> _mockService;
    private readonly AuthController _controller;
    private readonly IConfiguration _configuration;

    public AuthControllerTest()
    {
        _mockService = new Mock<IAuthService>();
        //string? to avoid compile error from nullable annotation difference string vs string?
        var settings = new Dictionary<string, string?>
        {
          {"Jwt:Key", "superextralongtextkeykeykeykeykeykeykeykkeykeykeykeykeykeykekykeykadsadwakdowakdowkaodkwaodkwadkwpoqakdopqwopqwdkpo"},
          {"Jwt:Issuer", "TestIssuer"},
          {"Jwt:Audience", "TestAudience"},
          {"Jwt:ExpireMinutes", "30"}
        };
        _configuration = new ConfigurationBuilder()
          .AddInMemoryCollection(settings)
          .Build();
        _controller = new AuthController(_mockService.Object, _configuration);
    }

    [Fact]
    public async Task Register_ReturnsCode201_WhenSuccessfulRegistration()
    {
        _mockService.Setup(s => s.Register("User", "UserPassword"))
          .ReturnsAsync(true);

        var result = await _controller.Register("User", "UserPassword");
        Assert.NotNull(result);
        var okResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(201, okResult.StatusCode);

        _mockService.Verify(s => s.Register("User", "UserPassword"), Times.Once());

    }

    [Fact]
    public async Task Register_ReturnsBadRequest_WhenUsernameIsEmpty()
    {
        var result = await _controller.Register("", "UserPassword");
        Assert.IsType<BadRequestObjectResult>(result);

        _mockService.Verify(s => s.Register("", "UserPassword"), Times.Never());
    }

    [Fact]
    public async Task Register_ReturnsBadRequest_WhenPasswordIsEmpty()
    {
        var result = await _controller.Register("User", "");
        Assert.IsType<BadRequestObjectResult>(result);

        _mockService.Verify(s => s.Register("User", ""), Times.Never());
    }

    [Fact]
    public async Task Register_ReturnsBadRequest_WhenUsernameAlreadyExists()
    {
        _mockService.Setup(s => s.Register("User", "UserPassword"))
          .ReturnsAsync(false);

        var result = await _controller.Register("User", "UserPassword");
        Assert.IsType<BadRequestObjectResult>(result);

        _mockService.Verify(s => s.Register("User", "UserPassword"), Times.Once());
    }


    [Fact]
    public async Task Login_ReturnsOkWithToken_WhenValidCredentials()
    {
        var user = new User
        {
            Id = 1,
            Username = "User",
            //Mocking IAuthService, not testing BCrypt
            PasswordHash = "UserPassword",
            Role = "User"
        };
        _mockService.Setup(s => s.Login("User", "UserPassword"))
          .ReturnsAsync(user);

        var result = await _controller.Login("User", "UserPassword");
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.NotNull(okResult.Value);
        //Serialize response value 
        var json = System.Text.Json.JsonSerializer.Serialize(okResult.Value);
        // check if response contains token field
        Assert.Contains("token", json);

        _mockService.Verify(s => s.Login("User", "UserPassword"), Times.Once());
    }

    [Fact]
    public async Task Login_ReturnsBadRequest_WhenUsernameIsEmpty()
    {
        var result = await _controller.Login("", "UserPassword");
        Assert.IsType<BadRequestObjectResult>(result);

        _mockService.Verify(s => s.Login("", "UserPassword"), Times.Never());
    }
    [Fact]
    public async Task Login_ReturnsBadRequest_WhenPasswordIsEmpty()
    {
        var result = await _controller.Login("User", "");
        Assert.IsType<BadRequestObjectResult>(result);

        _mockService.Verify(s => s.Login("User", ""), Times.Never());
    }

    [Fact]
    public async Task Login_ReturnsUnauthorized_WhenCredentialsDoNotMatch()
    {
        _mockService.Setup(s => s.Login("User", "UserPassword"))
          .ReturnsAsync((User?)null);

        var result = await _controller.Login("User", "UserPassword");
        var okResult = Assert.IsType<UnauthorizedObjectResult>(result);

        _mockService.Verify(s => s.Login("User", "UserPassword"), Times.Once());

    }
}
