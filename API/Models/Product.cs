namespace InventoryManagement.Models;

public class Product
{
    public int Id { get; set; }
    //string.Empty to fix nullable 
    public string Name { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal Price { get; set; }
}
