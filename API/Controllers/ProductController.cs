using InventoryManagement.Models;
using Microsoft.AspNetCore.Mvc;
using InventoryManagement.Service;
using Microsoft.AspNetCore.Authorization;
using InventoryManagement.Dtos;
namespace InventoryManagement.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductsController : ControllerBase
{
    private readonly ProductService _service;

    public ProductsController(ProductService service)
    {
        _service = service;
    }
    [Authorize]
    [HttpPost]
    public async Task<ActionResult<Product>> PostProduct(CreateProductDto productDto)
    {
        var createdProduct = await _service.AddProduct(productDto);
        return Ok(createdProduct);
    }

    [HttpGet]
    public async Task<IActionResult> GetProducts()
    {
        var listOfProducts = await _service.GetAllProducts();
        return Ok(listOfProducts);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Product>> GetProductById(int id)
    {
        var product = await _service.GetProductById(id);
        if (product == null)
        {
            return NotFound();
        }
        return Ok(product);
    }
    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteProductById(int id)
    {
        var product = await _service.DeleteProductById(id);
        if (!product)
        {
            return NotFound();
        }
        return NoContent();
    }
    [Authorize]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProductById(int id, UpdateProductDto productDto)
    {
        var updatedProduct = await _service.UpdateProductById(id, productDto);
        if (updatedProduct == null)
        {
            return NotFound();
        }

        return NoContent();
    }

}
