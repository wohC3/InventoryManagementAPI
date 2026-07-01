using InventoryManagement.Models;
using InventoryManagement.Data;

namespace InventoryManagement.Service;


public class ProductService
{

    private AppDbContext _context;

    public ProductService(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddProduct(Product product)
    {
        _context.Products.Add(product);
        await _context.SaveChangesAsync();
    }
}
