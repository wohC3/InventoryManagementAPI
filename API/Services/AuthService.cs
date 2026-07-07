
using InventoryManagement.Models;
using InventoryManagement.Data;
using Microsoft.EntityFrameworkCore;
using BCrypt.Net;

namespace InventoryManagement.Service;


public class AuthService
{

    private readonly AppDbContext _context;

    public AuthService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Register(string username, string password)
    {
        //Lambda expressions tells EF Core about the condition to search for in the Users table.
        var userFound = await _context.Users.FirstOrDefaultAsync(user => user.Username == username);

        if (userFound != null)
        {
            return false;
        }
        var passwordHash = BCrypt.Net.BCrypt.HashPassword(password);
        var user = new User
        {
            Username = username,
            PasswordHash = passwordHash
        };
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return true;
    }

}

