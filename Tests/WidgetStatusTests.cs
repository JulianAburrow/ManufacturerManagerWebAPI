namespace Tests;

public class WidgetStatusTests
{
    [Fact]
    public async Task GetWidgetStatuses_GetsWidgetStatuses()
    {
        var testWidgetStatus1 = "TestWidgetStatus1";
        var testWidgetStatus2 = "TestWidgetStatus2";
        var testWidgetStatus3 = "TestWidgetStatus3";

    var context = Shared.GetInMemoryDbContext();
        context.WidgetStatuses.RemoveRange(context.WidgetStatuses);
        await context.SaveChangesAsync();

        context.WidgetStatuses.AddRange(new List<WidgetStatusModel>
        {
            new() { StatusName = testWidgetStatus1 },
            new() { StatusName = testWidgetStatus2 },
            new() { StatusName = testWidgetStatus3 },
        });

        await context.SaveChangesAsync();

        var controller = new WidgetStatusController(context);

        var result = await controller.GetWidgetStatuses();

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var actualWidgetStatuses = Assert.IsType<List<WidgetStatusDTO>>(okResult.Value);
        Assert.Equal(3, actualWidgetStatuses.Count);
        Assert.Equal(testWidgetStatus1, actualWidgetStatuses[0].StatusName);
        Assert.Equal(testWidgetStatus2, actualWidgetStatuses[1].StatusName);
        Assert.Equal(testWidgetStatus3, actualWidgetStatuses[2].StatusName);
    }
}
