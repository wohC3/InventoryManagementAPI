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

    public async Task<PageProductDto> GetAllProducts(int page, int pageSize)
    {
        var products = await _context.Products
          .OrderBy(p => p.Id)
          .Skip((page - 1) * pageSize)
          .Take(pageSize)
          .ToListAsync();

        var totalItems = await _context.Products.CountAsync();
        // Round up to include a final page when remaining products don't fill a full page.
        var totalPages = (totalItems + pageSize - 1) / pageSize;
        var productsDto = new PageProductDto
        {
            Page = page,
            PageSize = pageSize,
            TotalItems = totalItems,
            TotalPages = totalPages,
            Products = products
        };

        return productsDto;
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
