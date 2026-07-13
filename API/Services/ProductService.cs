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

    public async Task<ProductDto> AddProduct(CreateProductDto createProductDto)
    {
        var product = new Product
        {
            Name = createProductDto.Name,
            Quantity = createProductDto.Quantity,
            Price = createProductDto.Price
        };
        _context.Products.Add(product);
        await _context.SaveChangesAsync();
        //after savechangesasync the Id exists so we map product -> productDto.
        var result = new ProductDto
        {
            Id = product.Id,
            Name = product.Name,
            Quantity = product.Quantity,
            Price = product.Price
        };
        return result;
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
        var productDtos = products.Select(product => new ProductDto
        {
            Id = product.Id,
            Name = product.Name,
            Quantity = product.Quantity,
            Price = product.Price
        }).ToList();
        var pageProductsDto = new PageProductDto
        {
            Page = page,
            PageSize = pageSize,
            TotalItems = totalItems,
            TotalPages = totalPages,
            Products = productDtos
        };

        return pageProductsDto;
    }

    public async Task<ProductDto?> GetProductById(int id)
    {
        var productFound = await _context.Products.FindAsync(id);
        if (productFound == null)
        {
            return null;
        }
        var result = new ProductDto
        {
            Id = productFound.Id,
            Name = productFound.Name,
            Quantity = productFound.Quantity,
            Price = productFound.Price
        };
        return result;
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


    public async Task<ProductDto?> UpdateProductById(int id, UpdateProductDto productDto)
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
        var result = new ProductDto
        {
            Id = productToBeUpdated.Id,
            Name = productToBeUpdated.Name,
            Quantity = productToBeUpdated.Quantity,
            Price = productToBeUpdated.Price
        };
        return result;
    }
}
