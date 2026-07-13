using InventoryManagement.Models;

namespace InventoryManagement.Dtos;

public class PageProductDto
{
    public int Page { get; set; }

    public int PageSize { get; set; }

    public int TotalItems { get; set; }

    public int TotalPages { get; set; }

    public List<Product> Products { get; set; } = [];
}
