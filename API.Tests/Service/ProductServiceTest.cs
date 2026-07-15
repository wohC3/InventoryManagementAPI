using InventoryManagement.Service;
using InventoryManagement.Data;
using InventoryManagement.Dtos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.Sqlite;
using Xunit;


namespace API.Tests.Service;

public class ProductServiceTests
{
    [Fact]
    public async Task AddProduct_ShouldCreateProduct_WhenValidDataIsProvided()
    {
        //await using var to dispose of connection.
        await using var connection = new SqliteConnection("DataSource=:memory:");
        await connection.OpenAsync();

        var options = new DbContextOptionsBuilder<AppDbContext>()
          .UseSqlite(connection)
          .Options;
        //again await using var to dispose of context.
        await using var context = new AppDbContext(options);
        await context.Database.EnsureCreatedAsync();

        var service = new ProductService(context);

        var dto = new CreateProductDto
        {
            Name = "Phone",
            Quantity = 1,
            //decimal literal 'm' for decimal numbers
            Price = 299.99m
        };

        var result = await service.AddProduct(dto);

        Assert.NotNull(result);
        Assert.Equal("Phone", result.Name);
        Assert.Equal(1, result.Quantity);
        Assert.Equal(299.99m, result.Price);
        Assert.True(result.Id > 0);

        var savedProduct = await context.Products.FirstOrDefaultAsync();
        Assert.NotNull(savedProduct);
        Assert.Equal("Phone", savedProduct.Name);
        Assert.Equal(1, savedProduct.Quantity);
        Assert.Equal(299.99m, savedProduct.Price);
        Assert.True(savedProduct.Id > 0);
    }

    [Fact]
    public async Task GetProductById_ShouldReturnProductById_WhenValidIdIsProvided()
    {
        //MANUAL APPDBCONTEXT DI
        //await using var to dispose of connection.
        await using var connection = new SqliteConnection("DataSource=:memory:");
        await connection.OpenAsync();
        var options = new DbContextOptionsBuilder<AppDbContext>()
          .UseSqlite(connection)
          .Options;
        //again await using var to dispose of context.
        await using var context = new AppDbContext(options);
        await context.Database.EnsureCreatedAsync();
        var service = new ProductService(context);
        //MANUAL APPDBCONTEXT DI


        var dto = new CreateProductDto
        {
            Name = "Phone",
            Quantity = 1,
            //decimal literal 'm' for decimal numbers
            Price = 299.99m
        };

        var createdProduct = await service.AddProduct(dto);
        var foundProductById = await service.GetProductById(createdProduct.Id);

        Assert.NotNull(foundProductById);
        Assert.Equal("Phone", foundProductById.Name);
        Assert.Equal(1, foundProductById.Quantity);
        Assert.Equal(299.99m, foundProductById.Price);
        Assert.Equal(createdProduct.Id, foundProductById.Id);
    }

    [Fact]
    public async Task GetAllProducts_ShouldReturnPagedProducts_WhenProductsExist()
    {
        //MANUAL APPDBCONTEXT DI
        //await using var to dispose of connection.
        await using var connection = new SqliteConnection("DataSource=:memory:");
        await connection.OpenAsync();
        var options = new DbContextOptionsBuilder<AppDbContext>()
          .UseSqlite(connection)
          .Options;
        //again await using var to dispose of context.
        await using var context = new AppDbContext(options);
        await context.Database.EnsureCreatedAsync();
        var service = new ProductService(context);
        //MANUAL APPDBCONTEXT DI

        for (int i = 1; i < 6; i++)
        {
            var dto = new CreateProductDto
            {
                Name = "Phone" + i,
                Quantity = i,
                //decimal literal 'm' for decimal numbers
                Price = 299.99m
            };
            await service.AddProduct(dto);
        }
        //page 2 and pagesize 2 should return phone3 and phone4 (1,2 are skipped)
        var pageResult = await service.GetAllProducts(2, 2);

        Assert.NotNull(pageResult);

        Assert.Equal(pageResult.Page, 2);
        Assert.Equal(pageResult.PageSize, 2);
        Assert.Equal(pageResult.TotalItems, 5);
        Assert.Equal(pageResult.TotalPages, 3);

        //phone 3 
        Assert.Equal("Phone3", pageResult.Products[0].Name);
        Assert.Equal(3, pageResult.Products[0].Quantity);
        Assert.Equal(299.99m, pageResult.Products[0].Price);
        //phone 4
        Assert.Equal("Phone4", pageResult.Products[1].Name);
        Assert.Equal(4, pageResult.Products[1].Quantity);
        Assert.Equal(299.99m, pageResult.Products[1].Price);
    }


    [Fact]
    public async Task UpdateProductById_ShouldUpdateProduct_WhenProductExists()
    {
        //await using var to dispose of connection.
        await using var connection = new SqliteConnection("DataSource=:memory:");
        await connection.OpenAsync();

        var options = new DbContextOptionsBuilder<AppDbContext>()
          .UseSqlite(connection)
          .Options;
        //again await using var to dispose of context.
        await using var context = new AppDbContext(options);
        await context.Database.EnsureCreatedAsync();

        var service = new ProductService(context);

        var dto = new CreateProductDto
        {
            Name = "Phone",
            Quantity = 1,
            //decimal literal 'm' for decimal numbers
            Price = 299.99m
        };

        var createdProduct = await service.AddProduct(dto);

        var result = await service.UpdateProductById(createdProduct.Id,
            new UpdateProductDto
            {
                Name = "PhoneUpdated",
                Quantity = 2,
                Price = 300.99m
            });

        Assert.NotNull(result);
        Assert.Equal("PhoneUpdated", result.Name);
        Assert.Equal(2, result.Quantity);
        Assert.Equal(300.99m, result.Price);

        var savedProduct = await context.Products.FindAsync(result.Id);
        Assert.NotNull(savedProduct);
        Assert.Equal("PhoneUpdated", savedProduct.Name);
        Assert.Equal(2, savedProduct.Quantity);
        Assert.Equal(300.99m, savedProduct.Price);

    }


    [Fact]
    public async Task DeleteProduct_ShouldDeleteProduct_WhenProductExists()
    {
        //await using var to dispose of connection.
        await using var connection = new SqliteConnection("DataSource=:memory:");
        await connection.OpenAsync();
        var options = new DbContextOptionsBuilder<AppDbContext>()
          .UseSqlite(connection)
          .Options;
        //again await using var to dispose of context.
        await using var context = new AppDbContext(options);
        await context.Database.EnsureCreatedAsync();
        var service = new ProductService(context);

        var dto = new CreateProductDto
        {
            Name = "Phone",
            Quantity = 1,
            //decimal literal 'm' for decimal numbers
            Price = 299.99m
        };

        var createdProduct = await service.AddProduct(dto);

        var result = await service.DeleteProductById(createdProduct.Id);

        Assert.True(result);

        var deletedProduct = await context.Products.FindAsync(createdProduct.Id);
        Assert.Null(deletedProduct);
    }


    [Fact]
    public async Task GetProductById_ShouldReturnNull_WhenProductDoesNotExist()
    {
        //MANUAL APPDBCONTEXT DI
        //await using var to dispose of connection.
        await using var connection = new SqliteConnection("DataSource=:memory:");
        await connection.OpenAsync();
        var options = new DbContextOptionsBuilder<AppDbContext>()
          .UseSqlite(connection)
          .Options;
        //again await using var to dispose of context.
        await using var context = new AppDbContext(options);
        await context.Database.EnsureCreatedAsync();
        var service = new ProductService(context);
        //MANUAL APPDBCONTEXT DI

        var nonExistingId = 1;
        var foundProductById = await service.GetProductById(nonExistingId);

        Assert.Null(foundProductById);
    }

    [Fact]
    public async Task UpdateProductById_ShouldReturnNull_WhenProductDoesNotExist()
    {
        //await using var to dispose of connection.
        await using var connection = new SqliteConnection("DataSource=:memory:");
        await connection.OpenAsync();

        var options = new DbContextOptionsBuilder<AppDbContext>()
          .UseSqlite(connection)
          .Options;
        //again await using var to dispose of context.
        await using var context = new AppDbContext(options);
        await context.Database.EnsureCreatedAsync();

        var service = new ProductService(context);

        var nonExistingId = 1;
        var result = await service.UpdateProductById(nonExistingId,
            new UpdateProductDto
            {
                Name = "PhoneUpdated",
                Quantity = 2,
                Price = 300.99m
            });

        Assert.Null(result);
        var savedProduct = await context.Products.FindAsync(nonExistingId);
        Assert.Null(savedProduct);

    }


    [Fact]
    public async Task DeleteProduct_ShouldReturnFalse_WhenProductDoesNotExist()
    {
        //await using var to dispose of connection.
        await using var connection = new SqliteConnection("DataSource=:memory:");
        await connection.OpenAsync();
        var options = new DbContextOptionsBuilder<AppDbContext>()
          .UseSqlite(connection)
          .Options;
        //again await using var to dispose of context.
        await using var context = new AppDbContext(options);
        await context.Database.EnsureCreatedAsync();
        var service = new ProductService(context);

        var nonExistingId = 1;

        var deleteResult = await service.DeleteProductById(nonExistingId);
        Assert.False(deleteResult);
    }
}
