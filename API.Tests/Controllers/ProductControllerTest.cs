using InventoryManagement.Controllers;
using InventoryManagement.Interfaces;
using Moq;
using InventoryManagement.Dtos;
using Microsoft.AspNetCore.Mvc;




namespace API.Tests.Controllers;

public class ProductControllerTest
{
    private readonly Mock<IProductService> _mockService;
    private readonly ProductsController _controller;


    public ProductControllerTest()
    {
        _mockService = new Mock<IProductService>();
        _controller = new ProductsController(_mockService.Object);
    }


    [Fact]
    public async Task GetProductById_ReturnsOk_WhenProductExists()
    {
        var productDto = new ProductDto
        {
            Id = 1,
            Name = "Phone",
            Quantity = 1,
            Price = 299.99m
        };

        _mockService.Setup(s => s.GetProductById(1))
          .ReturnsAsync(productDto);

        var result = await _controller.GetProductById(1);
        Assert.NotNull(result.Result);
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var response = Assert.IsType<ProductDto>(okResult.Value);
        Assert.Equal(1, response.Id);
        Assert.Equal("Phone", response.Name);
        Assert.Equal(1, response.Quantity);
        Assert.Equal(299.99m, response.Price);
        //checks if service was called
        _mockService.Verify(s => s.GetProductById(1), Times.Once());
    }
    [Fact]
    public async Task GetProductById_ReturnsNotFound_WhenProductDoesNotExist()
    {
        _mockService.Setup(s => s.GetProductById(1))
          //casting null to be of type ProductDto
          .ReturnsAsync((ProductDto?)null);

        var result = await _controller.GetProductById(1);
        Assert.IsType<NotFoundResult>(result.Result);

        _mockService.Verify(s => s.GetProductById(1), Times.Once());
    }

    [Fact]
    public async Task GetProducts_ReturnsPagedProducts_WhenProductsExist()
    {
        var items = new List<ProductDto>();
        for (int i = 1; i < 6; i++)
        {
            var dto = new ProductDto
            {
                Name = "Phone" + i,
                Quantity = i,
                //decimal literal 'm' for decimal numbers
                Price = 299.99m
            };
            items.Add(dto);
        }

        var pageProductDto = new PageProductDto
        {
            Page = 2,
            PageSize = 2,
            TotalItems = 5,
            TotalPages = 3,
            Products = items
        };


        _mockService.Setup(s => s.GetAllProducts(2, 2))
          .ReturnsAsync(pageProductDto);

        var result = await _controller.GetProducts(2, 2);
        var okResult = Assert.IsType<OkObjectResult>(result);
        var response = Assert.IsType<PageProductDto>(okResult.Value);

        Assert.Equal(2, response.Page);
        Assert.Equal(2, response.PageSize);
        Assert.Equal(5, response.TotalItems);
        Assert.Equal(3, response.TotalPages);
        Assert.Equal(5, response.Products.Count);
        //checks if service was called
        _mockService.Verify(s => s.GetAllProducts(2, 2), Times.Once());
    }

    [Fact]
    public async Task GetProducts_ReturnsBadRequest_WhenPageIsLessThanOne()
    {
        var result = await _controller.GetProducts(0, 2);
        Assert.IsType<BadRequestObjectResult>(result);
        _mockService.Verify(s => s.GetAllProducts(0, 2), Times.Never());
    }

    [Fact]
    public async Task GetProducts_ReturnsBadRequest_WhenPageSizeIsMoreThanTen()
    {
        var result = await _controller.GetProducts(2, 11);
        Assert.IsType<BadRequestObjectResult>(result);
        _mockService.Verify(s => s.GetAllProducts(2, 11), Times.Never());
    }


    [Fact]
    public async Task GetProducts_ReturnsBadRequest_WhenPageSizeIsLessThanOne()
    {
        var result = await _controller.GetProducts(2, 0);
        Assert.IsType<BadRequestObjectResult>(result);
        _mockService.Verify(s => s.GetAllProducts(2, 0), Times.Never());
    }

    [Fact]
    public async Task PostProduct_ReturnsOk_WhenValidData()
    {
        var createProductDto = new CreateProductDto
        {
            Name = "Phone",
            Quantity = 1,
            Price = 299.99m
        };
        var productDto = new ProductDto
        {
            Id = 1,
            Name = "Phone",
            Quantity = 1,
            Price = 299.99m
        };

        _mockService.Setup(s => s.AddProduct(createProductDto))
          .ReturnsAsync(productDto);

        var result = await _controller.PostProduct(createProductDto);

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var response = Assert.IsType<ProductDto>(okResult.Value);

        Assert.Equal("Phone", response.Name);
        Assert.Equal(1, response.Quantity);
        Assert.Equal(299.99m, response.Price);
        //checks if service was called
        _mockService.Verify(s => s.AddProduct(createProductDto), Times.Once());
    }

    [Fact]
    public async Task UpdateProductById_ReturnsOk_WhenProductExists()
    {
        var updateProductDto = new UpdateProductDto
        {
            Name = "Phone",
            Quantity = 1,
            Price = 299.99m
        };

        var productDto = new ProductDto
        {
            Id = 1,
            Name = "Phone",
            Quantity = 1,
            Price = 299.99m
        };
        _mockService.Setup(s => s.UpdateProductById(1, updateProductDto))
          .ReturnsAsync(productDto);

        var result = await _controller.UpdateProductById(1, updateProductDto);
        Assert.NotNull(result.Result);
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var response = Assert.IsType<ProductDto>(okResult.Value);
        Assert.Equal(1, response.Id);
        Assert.Equal("Phone", response.Name);
        Assert.Equal(1, response.Quantity);
        Assert.Equal(299.99m, response.Price);
        //checks if service was called
        _mockService.Verify(s => s.UpdateProductById(1, updateProductDto), Times.Once());
    }

    [Fact]
    public async Task UpdateProductById_ReturnsNotFound_WhenProductDoesNotExist()
    {
        var updateProductDto = new UpdateProductDto
        {
            Name = "Phone",
            Quantity = 1,
            Price = 299.99m
        };
        _mockService.Setup(s => s.UpdateProductById(1, updateProductDto))
          .ReturnsAsync((ProductDto?)null);
        var result = await _controller.UpdateProductById(1, updateProductDto);
        Assert.IsType<NotFoundResult>(result.Result);
        _mockService.Verify(s => s.UpdateProductById(1, updateProductDto), Times.Once());
    }

    [Fact]
    public async Task DeleteProductById_ReturnsNoContent_WhenProductExists()
    {
        _mockService.Setup(s => s.DeleteProductById(1))
          .ReturnsAsync(true);
        var result = await _controller.DeleteProductById(1);
        Assert.IsType<NoContentResult>(result);
        _mockService.Verify(s => s.DeleteProductById(1), Times.Once());
    }

    [Fact]
    public async Task DeleteProductById_ReturnsNotFound_WhenProductDoesNotExist()
    {
        _mockService.Setup(s => s.DeleteProductById(1))
          .ReturnsAsync(false);
        var result = await _controller.DeleteProductById(1);
        Assert.IsType<NotFoundResult>(result);
        _mockService.Verify(s => s.DeleteProductById(1), Times.Once());
    }
}


