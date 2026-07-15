using InventoryManagement.Service;
using InventoryManagement.Data;
using InventoryManagement.Dtos;
using InventoryManagement.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.Sqlite;
using Xunit;

namespace API.Tests.Service;

public class UserServiceTest
{
    [Fact]
    public async Task UpdateUserRole_ShouldUpdateUserRole_WhenUserExists()
    {
        //await using var to dispose of connection.
        await using var connection = new SqliteConnection("DataSource=:memory:");
        await connection.OpenAsync();

        var options = new DbContextOptionsBuilder<AppDbContext>()
          .UseSqlite(connection)
          .Options;
        //again await using var to dispose of context.
        await using var context = new AppDbContext(options);
        await context.Database.EnsureCreatedAsync();

        var service = new UserService(context);

        var user = new User
        {
            Username = "Bob",
            PasswordHash = "PasswordHash",
            Role = "User"
        };

        context.Users.Add(user);
        await context.SaveChangesAsync();

        var dto = new UpdateUserRoleDto
        {
            Role = "Admin",
        };

        var result = await service.UpdateUserRole(user.Id, dto);

        Assert.NotNull(result);
        Assert.Equal(user.Id, result.Id);
        Assert.Equal("Bob", result.Username);
        Assert.Equal("Admin", result.Role);

        var savedUser = await context.Users.FindAsync(result.Id);
        Assert.NotNull(savedUser);
        Assert.Equal("Bob", savedUser.Username);
        Assert.Equal("Admin", savedUser.Role);
    }



    [Fact]
    public async Task UpdateUserRole_ShouldReturnNull_WhenUserDoesNotExist()
    {
        //await using var to dispose of connection.
        await using var connection = new SqliteConnection("DataSource=:memory:");
        await connection.OpenAsync();

        var options = new DbContextOptionsBuilder<AppDbContext>()
          .UseSqlite(connection)
          .Options;
        //again await using var to dispose of context.
        await using var context = new AppDbContext(options);
        await context.Database.EnsureCreatedAsync();

        var service = new UserService(context);

        var dto = new UpdateUserRoleDto
        {
            Role = "Admin",
        };
        var nonExistingId = 1;
        var result = await service.UpdateUserRole(nonExistingId, dto);

        Assert.Null(result);
        //check for accidental user creation.
        Assert.Equal(0, await context.Users.CountAsync());
    }


    [Fact]
    public async Task GetAllUsers_ShouldReturnUsersList_WhenUsersExist()
    {
        //MANUAL APPDBCONTEXT DI
        //await using var to dispose of connection.
        await using var connection = new SqliteConnection("DataSource=:memory:");
        await connection.OpenAsync();
        var options = new DbContextOptionsBuilder<AppDbContext>()
          .UseSqlite(connection)
          .Options;
        //again await using var to dispose of context.
        await using var context = new AppDbContext(options);
        await context.Database.EnsureCreatedAsync();
        var service = new UserService(context);
        //MANUAL APPDBCONTEXT DI

        for (int i = 1; i < 6; i++)
        {
            var user = new User
            {
                Username = "User" + i,
                PasswordHash = "PasswordHash",
                Role = "User"
            };
            context.Users.Add(user);
        }
        await context.SaveChangesAsync();
        var result = await service.GetAllUsers();

        Assert.NotNull(result);
        Assert.Equal(5, await context.Users.CountAsync());
        //user1 
        Assert.Equal("User1", result[0].Username);
        Assert.Equal("User", result[0].Role);
        //user3
        Assert.Equal("User3", result[2].Username);
        Assert.Equal("User", result[2].Role);
    }


    [Fact]
    public async Task GetAllUsers_ShouldReturnEmptyList_WhenUsersDoNotExist()
    {
        //MANUAL APPDBCONTEXT DI
        //await using var to dispose of connection.
        await using var connection = new SqliteConnection("DataSource=:memory:");
        await connection.OpenAsync();
        var options = new DbContextOptionsBuilder<AppDbContext>()
          .UseSqlite(connection)
          .Options;
        //again await using var to dispose of context.
        await using var context = new AppDbContext(options);
        await context.Database.EnsureCreatedAsync();
        var service = new UserService(context);
        //MANUAL APPDBCONTEXT DI

        var result = await service.GetAllUsers();

        Assert.NotNull(result);
        //for empty list return.
        Assert.Empty(result);
    }


}
