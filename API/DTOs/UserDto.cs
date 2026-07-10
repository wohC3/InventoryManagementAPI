namespace InventoryManagement.Dtos;

public class UserDto
{
    public int Id { get; set; }
    //string.Empty to fix nullable 
    public string Username { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
}

