namespace InventoryManagement.Data;

using Microsoft.EntityFrameworkCore;
using InventoryManagement.Models;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
      : base(options) { }

    // = null! to fix nullable warning
    public DbSet<Product> Products { get; set; } = null!;
    public DbSet<User> Users { get; set; } = null!;
}
