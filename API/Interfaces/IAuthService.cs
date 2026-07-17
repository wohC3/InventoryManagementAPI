using InventoryManagement.Models;

namespace InventoryManagement.Interfaces;

public interface IAuthService
{

    Task<bool> Register(string username, string password);

    Task<User?> Login(string username, string password);


}

