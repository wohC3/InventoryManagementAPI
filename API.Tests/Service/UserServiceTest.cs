using InventoryManagement.Service;
using InventoryManagement.Dtos;
using InventoryManagement.Models;
using Microsoft.EntityFrameworkCore;
using API.Tests.Helpers;

namespace API.Tests.Service;

public class UserServiceTest
{
    [Fact]
    public async Task UpdateUserRole_ShouldUpdateUserRole_WhenUserExists()
    {
        await using var db = await TestDatabase.CreateAsync();
        var service = new UserService(db.Context);

        var user = new User
        {
            Username = "Bob",
            PasswordHash = "PasswordHash",
            Role = "User"
        };

        db.Context.Users.Add(user);
        await db.Context.SaveChangesAsync();

        var dto = new UpdateUserRoleDto
        {
            Role = "Admin",
        };

        var result = await service.UpdateUserRole(user.Id, dto);

        Assert.NotNull(result);
        Assert.Equal(user.Id, result.Id);
        Assert.Equal("Bob", result.Username);
        Assert.Equal("Admin", result.Role);

        var savedUser = await db.Context.Users.FindAsync(result.Id);
        Assert.NotNull(savedUser);
        Assert.Equal("Bob", savedUser.Username);
        Assert.Equal("Admin", savedUser.Role);
    }



    [Fact]
    public async Task UpdateUserRole_ShouldReturnNull_WhenUserDoesNotExist()
    {
        await using var db = await TestDatabase.CreateAsync();
        var service = new UserService(db.Context);

        var dto = new UpdateUserRoleDto
        {
            Role = "Admin",
        };
        var nonExistingId = 1;
        var result = await service.UpdateUserRole(nonExistingId, dto);

        Assert.Null(result);
        //check for accidental user creation.
        Assert.Equal(0, await db.Context.Users.CountAsync());
    }


    [Fact]
    public async Task GetAllUsers_ShouldReturnUsersList_WhenUsersExist()
    {
        await using var db = await TestDatabase.CreateAsync();
        var service = new UserService(db.Context);

        for (int i = 1; i < 6; i++)
        {
            var user = new User
            {
                Username = "User" + i,
                PasswordHash = "PasswordHash",
                Role = "User"
            };
            await db.Context.Users.AddAsync(user);
        }
        await db.Context.SaveChangesAsync();
        var result = await service.GetAllUsers();

        Assert.NotNull(result);
        Assert.Equal(5, await db.Context.Users.CountAsync());
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
        await using var db = await TestDatabase.CreateAsync();
        var service = new UserService(db.Context);

        var result = await service.GetAllUsers();

        Assert.NotNull(result);
        //for empty list return.
        Assert.Empty(result);
    }
}
