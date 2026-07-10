using InventoryManagement.Models;
using InventoryManagement.Data;
using Microsoft.EntityFrameworkCore;
using InventoryManagement.Dtos;


namespace InventoryManagement.Service;

public class UserService

{

    private readonly AppDbContext _context;

    public UserService(AppDbContext context)
    {
        _context = context;
    }


    public async Task<UserDto?> UpdateUserRole(int id, UpdateUserRoleDto userRoleDto)
    {
        var userRoleToBeUpdated = await _context.Users.FindAsync(id);
        if (userRoleToBeUpdated == null)
        {
            return null;
        }

        userRoleToBeUpdated.Role = userRoleDto.Role;
        await _context.SaveChangesAsync();
        var user = new UserDto
        {
            Id = userRoleToBeUpdated.Id,
            Username = userRoleToBeUpdated.Username,
            Role = userRoleToBeUpdated.Role
        };

        return user;
    }


    public async Task<List<UserDto>> GetAllUsers()
    {
        var users = await _context.Users.ToListAsync();
        //map each user to userDto to avoid exposing sensitive data like passwordhash.
        var userDtos = users.Select(user => new UserDto
        {
            Id = user.Id,
            Username = user.Username,
            Role = user.Role
        }).ToList();
        return userDtos;
    }
}

