using InventoryManagement.Models;
using InventoryManagement.Data;
using Microsoft.EntityFrameworkCore;

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

    public async Task<List<Product>> GetAllProducts()
    {
        return await _context.Products.ToListAsync();
    }

    public async Task<Product?> GetProductById(int id)
    {
        var productFound = await _context.Products.FindAsync(id);
        return productFound;
    }

    public async Task<bool> DeleteProductById(int id)
    {
        var productToBeDeleted = await _context.Products.FindAsync(id);
        if (productToBeDeleted == null)
        {
            return false;
        }
        _context.Products.Remove(productToBeDeleted);
        await _context.SaveChangesAsync();
        return true;
    }
}
