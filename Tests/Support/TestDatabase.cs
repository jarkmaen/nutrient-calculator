using Backend.Data;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace Tests.Support;

public sealed class TestDatabase : IDisposable
{
    private readonly SqliteConnection connection = new("DataSource=:memory:");

    public AppDbContext Context { get; }

    public TestDatabase()
    {
        connection.Open();

        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite(connection)
            .Options;

        Context = new AppDbContext(options);
        Context.Database.EnsureCreated();
    }

    public void Dispose()
    {
        Context.Dispose();
        connection.Dispose();
    }
}
