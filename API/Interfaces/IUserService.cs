using InventoryManagement.Dtos;

namespace InventoryManagement.Interfaces;

public interface IUserService
{
    Task<UserDto?> UpdateUserRole(int id, UpdateUserRoleDto userRoleDto);

    Task<List<UserDto>> GetAllUsers();
}
