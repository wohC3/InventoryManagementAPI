using InventoryManagement.Controllers;
using InventoryManagement.Interfaces;
using Moq;
using InventoryManagement.Dtos;
using Microsoft.AspNetCore.Mvc;




namespace API.Tests.Controllers;

public class UsersControllerTest
{
    private readonly Mock<IUserService> _mockService;
    private readonly UsersController _controller;

    public UsersControllerTest()
    {
        _mockService = new Mock<IUserService>();
        _controller = new UsersController(_mockService.Object);
    }

    [Fact]
    public async Task UpdateUserRoleById_ReturnsOk_WhenUserExists()
    {
        var userRoleDto = new UpdateUserRoleDto
        {
            Role = "User"
        };

        var userDto = new UserDto
        {
            Id = 1,
            Username = "Bob",
            Role = "User",
        };
        _mockService.Setup(s => s.UpdateUserRole(1, userRoleDto))
          .ReturnsAsync(userDto);

        var result = await _controller.UpdateUserRoleById(1, userRoleDto);
        Assert.NotNull(result);
        var okResult = Assert.IsType<OkObjectResult>(result);
        var response = Assert.IsType<UserDto>(okResult.Value);
        Assert.Equal(1, response.Id);
        Assert.Equal("Bob", response.Username);
        Assert.Equal("User", response.Role);
        //checks if service was called
        _mockService.Verify(s => s.UpdateUserRole(1, userRoleDto), Times.Once());

    }


    [Fact]
    public async Task UpdateUserRoleById_ReturnsNotFound_WhenUserDoesNotExist()
    {

        var userRoleDto = new UpdateUserRoleDto
        {
            Role = "User"
        };

        _mockService.Setup(s => s.UpdateUserRole(1, userRoleDto))
          .ReturnsAsync((UserDto?)null);

        var result = await _controller.UpdateUserRoleById(1, userRoleDto);
        Assert.IsType<NotFoundResult>(result);
        //checks if service was called
        _mockService.Verify(s => s.UpdateUserRole(1, userRoleDto), Times.Once());
    }

    [Fact]
    public async Task GetUsers_ReturnsUsers_WhenUsersExist()
    {
        var users = new List<UserDto>();
        for (int i = 1; i < 6; i++)
        {
            var dto = new UserDto
            {
                Id = i,
                Username = "Bob" + i,
                Role = "User",
            };
            users.Add(dto);
        }
        _mockService.Setup(s => s.GetAllUsers())
          .ReturnsAsync(users);

        var result = await _controller.GetUsers();
        var okResult = Assert.IsType<OkObjectResult>(result);
        var response = Assert.IsType<List<UserDto>>(okResult.Value);
        Assert.Equal("Bob1", response[0].Username);
        Assert.Equal("Bob5", response[4].Username);
        Assert.Equal(5, response.Count);

        _mockService.Verify(s => s.GetAllUsers(), Times.Once());
    }

    [Fact]
    public async Task GetUsers_ReturnsEmptyList_WhenUsersDoNotExist()
    {
        var users = new List<UserDto>();
        _mockService.Setup(s => s.GetAllUsers())
          .ReturnsAsync(users);

        var result = await _controller.GetUsers();
        var okResult = Assert.IsType<OkObjectResult>(result);
        var response = Assert.IsType<List<UserDto>>(okResult.Value);
        Assert.Equal(0, response.Count);

        _mockService.Verify(s => s.GetAllUsers(), Times.Once());
    }
}
