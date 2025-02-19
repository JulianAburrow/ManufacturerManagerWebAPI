using WebAPI.Data;

namespace Tests;

public static class Shared
{
    public static ManufacturerManagerDbContext GetInMemoryDbContext()
    {
        var options = new DbContextOptionsBuilder<ManufacturerManagerDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        var context = new ManufacturerManagerDbContext(options);
        context.Database.EnsureCreated();

        return context;
    }
}
