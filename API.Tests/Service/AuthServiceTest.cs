using InventoryManagement.Service;
using InventoryManagement.Data;
using InventoryManagement.Dtos;
using InventoryManagement.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.Sqlite;
using Xunit;

namespace API.Tests.Service;

public class AuthServiceTest
{

    [Fact]
    public async Task Register_ShouldAddUserToDb_WhenValidDataIsProvided()
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

        var service = new AuthService(context);

        var result = await service.Register("Bob", "PasswordHash");

        Assert.True(result);

        var savedUser = await context.Users.FirstOrDefaultAsync();
        Assert.NotNull(savedUser);
        Assert.Equal("Bob", savedUser.Username);
        Assert.Equal("User", savedUser.Role);
        Assert.True(BCrypt.Net.BCrypt.Verify("PasswordHash", savedUser.PasswordHash));

    }


    [Fact]
    public async Task Register_ShouldReturnFalse_WhenUsernameAlreadyExists()
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

        var service = new AuthService(context);


        var firstResult = await service.Register("Bob", "PasswordHash");
        Assert.True(firstResult);
        await context.SaveChangesAsync();
        //Duplicate registration
        var resultDuplicate = await service.Register("Bob", "PasswordHash");
        Assert.False(resultDuplicate);
        var users = await context.Users.ToListAsync();
        //if there's only 1 user(with no duplicate).
        Assert.Single(users);
        Assert.Equal("Bob", users[0].Username);
    }


    [Fact]
    public async Task Login_ShouldReturnUser_WhenValidIsProvided()
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

        var service = new AuthService(context);
        var passwordHash = BCrypt.Net.BCrypt.HashPassword("PasswordHash");
        var user = new User
        {
            Username = "Bob",
            PasswordHash = passwordHash,
            Role = "User"
        };
        await context.Users.AddAsync(user);
        await context.SaveChangesAsync();

        var result = await service.Login(user.Username, "PasswordHash");

        Assert.NotNull(result);
        Assert.Equal("Bob", result.Username);
        Assert.Equal("User", result.Role);


    }

    [Fact]
    public async Task Login_ShouldReturnNull_WhenPasswordIsIncorrect()
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

        var service = new AuthService(context);


        var passwordHash = BCrypt.Net.BCrypt.HashPassword("PasswordHash");
        var user = new User
        {
            Username = "Bob",
            PasswordHash = passwordHash,
            Role = "User"
        };
        await context.Users.AddAsync(user);
        await context.SaveChangesAsync();

        var result = await service.Login(user.Username, "WrongPassword");

        Assert.Null(result);

    }


    [Fact]
    public async Task Login_ShouldReturnNull_WhenUsernameIsIncorrect()
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

        var service = new AuthService(context);


        var passwordHash = BCrypt.Net.BCrypt.HashPassword("PasswordHash");
        var user = new User
        {
            Username = "Bob",
            PasswordHash = passwordHash,
            Role = "User"
        };
        await context.Users.AddAsync(user);
        await context.SaveChangesAsync();

        var result = await service.Login("WrongUsername", "PasswordHash");

        Assert.Null(result);

    }



}
