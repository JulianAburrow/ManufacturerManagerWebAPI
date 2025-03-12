
namespace Tests;

public class WidgetStatusHandlerTests
{
    private readonly WidgetStatusHandler _handler;
    private readonly ManufacturerManagerDbContext _context;

    public WidgetStatusHandlerTests()
    {
        _context = Shared.GetInMemoryDbContext();
        _handler = new WidgetStatusHandler(_context);
    }

    [Fact]
    public async Task GetWidgetStatuses_GetsAllWidgetStatuses()
    {
        await RemoveAllWidgetStatusesFromContext();

        var testWidgetStatus1 = "TestWidgetStatus1";
        var testWidgetStatus2 = "TestWidgetStatus2";

        _context.WidgetStatuses.AddRange(new List<WidgetStatusModel>
        {
            new() { StatusName = testWidgetStatus1 },
            new() { StatusName = testWidgetStatus2 },
        });
        await _context.SaveChangesAsync();
        var result = await _handler.GetWidgetStatusesAsync();

        Assert.Equal(2, result.Count);
        Assert.Equal(testWidgetStatus1, result[0].StatusName);
        Assert.Equal(testWidgetStatus2, result[1].StatusName);
    }

    private async Task RemoveAllWidgetStatusesFromContext()
    {
        _context.WidgetStatuses.RemoveRange(_context.WidgetStatuses);
        await _context.SaveChangesAsync();
    }
}
