using InventoryManagement.Models;
using InventoryManagement.Data;
using Microsoft.EntityFrameworkCore;
using InventoryManagement.Dtos;

namespace InventoryManagement.Service;


public class ProductService
{

    private readonly AppDbContext _context;

    public ProductService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Product> AddProduct(CreateProductDto productDto)
    {
        var product = new Product
        {
            Name = productDto.Name,
            Quantity = productDto.Quantity,
            Price = productDto.Price
        };
        _context.Products.Add(product);
        await _context.SaveChangesAsync();
        return product;
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


    public async Task<Product?> UpdateProductById(int id, UpdateProductDto productDto)
    {
        var productToBeUpdated = await _context.Products.FindAsync(id);

        if (productToBeUpdated == null)
        {
            return null;
        }
        productToBeUpdated.Name = productDto.Name;
        productToBeUpdated.Quantity = productDto.Quantity;
        productToBeUpdated.Price = productDto.Price;

        await _context.SaveChangesAsync();
        return productToBeUpdated;
    }
}
