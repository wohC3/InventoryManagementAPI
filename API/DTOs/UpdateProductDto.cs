using System.ComponentModel.DataAnnotations;

namespace InventoryManagement.Dtos;

public class UpdateProductDto
{
    [Required(ErrorMessage = "Product name is required.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Product name must be between 2 and 100 characters.")]
    public string Name { get; set; } = string.Empty;
    [Range(0, int.MaxValue, ErrorMessage = "Quantity cannot be negative.")]
    public int Quantity { get; set; }
    [Range(typeof(decimal), "0.01", "1000000", ErrorMessage = "Price must be greater than 0.")]
    public decimal Price { get; set; }
}
