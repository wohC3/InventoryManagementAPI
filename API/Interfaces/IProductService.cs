using InventoryManagement.Dtos;

namespace InventoryManagement.Interfaces;

public interface IProductService
{
    Task<ProductDto> AddProduct(CreateProductDto dto);

    Task<PageProductDto> GetAllProducts(int page, int pageSize);

    Task<ProductDto?> GetProductById(int id);

    Task<ProductDto?> UpdateProductById(int id, UpdateProductDto dto);

    Task<bool> DeleteProductById(int id);
}
