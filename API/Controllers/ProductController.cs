using InventoryManagement.Models;
using Microsoft.AspNetCore.Mvc;
using InventoryManagement.Service;
using Microsoft.AspNetCore.Authorization;
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
    public async Task<ActionResult<Product>> PostProduct(Product product)
    {
        //Quantity can be 0 to track out of stock products.
        if (string.IsNullOrWhiteSpace(product.Name) || product.Price <= 0 || product.Quantity < 0)
        {
            return BadRequest(new
            {
                message = "Validation failed: Name required, Price must be above 0 or Quantity cannot be negative"
            });
        }
        await _service.AddProduct(product);
        return Ok(product);
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
    [Authorize]
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
    public async Task<IActionResult> UpdateProductById(int id, Product product)
    {
        //Quantity can be 0 to track out of stock products.
        if (string.IsNullOrWhiteSpace(product.Name) || product.Price <= 0 || product.Quantity < 0)
        {
            return BadRequest(new
            {
                message = "Validation failed: Name required, Price must be above 0 or Quantity cannot be negative"
            });
        }
        //check for id mismatch.
        if (id != product.Id)
        {
            return BadRequest("The route ID must match the product Id.");
        }
        var productToUpdate = await _service.UpdateProductById(id, product);
        if (productToUpdate == null)
        {
            return NotFound();
        }

        return NoContent();
    }

}
