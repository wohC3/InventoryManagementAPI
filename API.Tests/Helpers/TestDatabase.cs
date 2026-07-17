using Microsoft.EntityFrameworkCore;
using Microsoft.Data.Sqlite;
using InventoryManagement.Data;

namespace API.Tests.Helpers;
//sealed - helper class not intended for inheritance
public sealed class TestDatabase : IAsyncDisposable
{

    private readonly SqliteConnection _connection;

    public AppDbContext Context { get; }

    private TestDatabase(SqliteConnection connection, AppDbContext context)
    {
        _connection = connection;
        Context = context;
    }
    //static - we don't need existing DB to create one
    //Creates fresh in-memory SQLite database and initialized AppDbContext
    //for use in unit tests
    public static async Task<TestDatabase> CreateAsync()
    {
        var connection = new SqliteConnection("DataSource=:memory:");
        await connection.OpenAsync();

        var options = new DbContextOptionsBuilder<AppDbContext>()
          .UseSqlite(connection)
          .Options;

        var context = new AppDbContext(options);
        await context.Database.EnsureCreatedAsync();

        return new TestDatabase(connection, context);
    }
    //Cleans up by 'await using'
    public async ValueTask DisposeAsync()
    {
        await Context.DisposeAsync();
        await _connection.DisposeAsync();
    }

}

