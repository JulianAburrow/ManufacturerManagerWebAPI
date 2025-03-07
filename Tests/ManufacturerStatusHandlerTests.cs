namespace Tests;

public class ManufacturerStatusHandlerTests
{
    private readonly ManufacturerStatusHandler _handler;
    private readonly ManufacturerManagerDbContext _context;

    public ManufacturerStatusHandlerTests()
    {
        _context = Shared.GetInMemoryDbContext();
        _handler = new ManufacturerStatusHandler(_context);
    }

    [Fact]
    public async Task GetManufacturerStatuses_GetsAllManufacturerStatuses()
    {
        await RemoveAllManufacturerStatusesFromContext();

        var testManufacturerStatus1 = "TestManufacturerStatus1";
        var testManufacturerStatus2 = "TestManufacturerStatus2";

        _context.ManufacturerStatuses.AddRange(new List<ManufacturerStatusModel>
        {
            new() { StatusName = testManufacturerStatus1 },
            new() { StatusName = testManufacturerStatus2 },
        });
        await _context.SaveChangesAsync();

        var result = await _handler.GetManufacturerStatusesAsync();

        Assert.Equal(2, result.Count);
        Assert.Equal(testManufacturerStatus1, result[0].StatusName);
        Assert.Equal(testManufacturerStatus2, result[1].StatusName);
    }

    private async Task RemoveAllManufacturerStatusesFromContext()
    {
        _context.ManufacturerStatuses.RemoveRange(_context.ManufacturerStatuses);
        await _context.SaveChangesAsync();
    }
}
