using InventoryManagement.Service;
using InventoryManagement.Models;
using Microsoft.EntityFrameworkCore;
using API.Tests.Helpers;

namespace API.Tests.Service;

public class AuthServiceTest
{

    [Fact]
    public async Task Register_ShouldAddUserToDb_WhenValidDataIsProvided()
    {
        await using var db = await TestDatabase.CreateAsync();
        var service = new AuthService(db.Context);

        var result = await service.Register("Bob", "PasswordHash");

        Assert.True(result);

        var savedUser = await db.Context.Users.FirstOrDefaultAsync();
        Assert.NotNull(savedUser);
        Assert.Equal("Bob", savedUser.Username);
        Assert.Equal("User", savedUser.Role);
        Assert.True(BCrypt.Net.BCrypt.Verify("PasswordHash", savedUser.PasswordHash));

    }


    [Fact]
    public async Task Register_ShouldReturnFalse_WhenUsernameAlreadyExists()
    {
        await using var db = await TestDatabase.CreateAsync();
        var service = new AuthService(db.Context);

        var firstResult = await service.Register("Bob", "PasswordHash");
        Assert.True(firstResult);
        await db.Context.SaveChangesAsync();
        //Duplicate registration
        var resultDuplicate = await service.Register("Bob", "PasswordHash");
        Assert.False(resultDuplicate);
        var users = await db.Context.Users.ToListAsync();
        //if there's only 1 user(with no duplicate).
        Assert.Single(users);
        Assert.Equal("Bob", users[0].Username);
    }


    [Fact]
    public async Task Login_ShouldReturnUser_WhenValidIsProvided()
    {
        await using var db = await TestDatabase.CreateAsync();
        var service = new AuthService(db.Context);

        var passwordHash = BCrypt.Net.BCrypt.HashPassword("PasswordHash");
        var user = new User
        {
            Username = "Bob",
            PasswordHash = passwordHash,
            Role = "User"
        };
        await db.Context.Users.AddAsync(user);
        await db.Context.SaveChangesAsync();

        var result = await service.Login(user.Username, "PasswordHash");

        Assert.NotNull(result);
        Assert.Equal("Bob", result.Username);
        Assert.Equal("User", result.Role);


    }

    [Fact]
    public async Task Login_ShouldReturnNull_WhenPasswordIsIncorrect()
    {
        await using var db = await TestDatabase.CreateAsync();
        var service = new AuthService(db.Context);

        var passwordHash = BCrypt.Net.BCrypt.HashPassword("PasswordHash");
        var user = new User
        {
            Username = "Bob",
            PasswordHash = passwordHash,
            Role = "User"
        };
        await db.Context.Users.AddAsync(user);
        await db.Context.SaveChangesAsync();

        var result = await service.Login(user.Username, "WrongPassword");

        Assert.Null(result);

    }


    [Fact]
    public async Task Login_ShouldReturnNull_WhenUsernameIsIncorrect()
    {
        await using var db = await TestDatabase.CreateAsync();
        var service = new AuthService(db.Context);

        var passwordHash = BCrypt.Net.BCrypt.HashPassword("PasswordHash");
        var user = new User
        {
            Username = "Bob",
            PasswordHash = passwordHash,
            Role = "User"
        };
        await db.Context.Users.AddAsync(user);
        await db.Context.SaveChangesAsync();

        var result = await service.Login("WrongUsername", "PasswordHash");

        Assert.Null(result);

    }



}
