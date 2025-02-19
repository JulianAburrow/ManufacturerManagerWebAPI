namespace Tests;

public class ManufacturerStatusTests
{
    [Fact]
    public async Task GetManufacturerStatuses_GetsManufacturerStatuses()
    {
        var testManufacturerStatus1 = "TestManufacturerStatus1";
        var testManufacturerStatus2 = "TestManufacturerStatus2";
        var testManufacturerStatus3 = "TestManufacturerStatus3";

        var context = Shared.GetInMemoryDbContext();
        context.ManufacturerStatuses.RemoveRange(context.ManufacturerStatuses);
        await context.SaveChangesAsync();

        context.ManufacturerStatuses.AddRange(new List<ManufacturerStatusModel>
        {
            new() { StatusName = testManufacturerStatus1 },
            new() { StatusName = testManufacturerStatus2 },
            new() { StatusName = testManufacturerStatus3 },
        });

        await context.SaveChangesAsync();

        var controller = new ManufacturerStatusController(context);

        var result = await controller.GetManufacturerStatuses();

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var actualManufacturerStatuses = Assert.IsType<List<ManufacturerStatusDTO>>(okResult.Value);
        Assert.Equal(3, actualManufacturerStatuses.Count);
        Assert.Equal(testManufacturerStatus1, actualManufacturerStatuses[0].StatusName);
        Assert.Equal(testManufacturerStatus2, actualManufacturerStatuses[1].StatusName);
        Assert.Equal(testManufacturerStatus3, actualManufacturerStatuses[2].StatusName);
    }
}
