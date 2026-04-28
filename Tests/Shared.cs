namespace Tests;

public static class Shared
{
    public static ManufacturerManagerDbContext GetInMemoryDbContext()
    {
        var options = new DbContextOptionsBuilder<ManufacturerManagerDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        var context = new ManufacturerManagerDbContext(options);
        context.Database.EnsureCreated();

        return context;
    }
}
