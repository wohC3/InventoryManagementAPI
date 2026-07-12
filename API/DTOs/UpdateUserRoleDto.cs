using System.ComponentModel.DataAnnotations;

namespace InventoryManagement.Dtos;

public class UpdateUserRoleDto
{
    [Required(ErrorMessage = "Role is required.")]
    [RegularExpression("^(User|Admin)$", ErrorMessage = "Valid roles are User and Admin.")]
    public string Role { get; set; } = string.Empty;
}
