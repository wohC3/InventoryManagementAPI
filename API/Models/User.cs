namespace InventoryManagement.Models;


public class User
{
    public int Id { get; set; }
    //string.Empty to fix nullable 
    public string Username { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string Role { get; set; } = "User";
}
