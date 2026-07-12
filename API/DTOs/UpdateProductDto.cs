namespace InventoryManagement.Dtos;

public class UpdateProductDto
{
    public string Name { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal Price { get; set; }
}
