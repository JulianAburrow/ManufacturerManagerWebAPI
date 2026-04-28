namespace Tests;

public class ManufacturerStatusControllerTests
{
    private readonly Mock<IManufacturerStatusHandler> _mockManufacturerStatusHandler;
    private readonly ManufacturerStatusController _manufacturerStatusController;

    public ManufacturerStatusControllerTests()
    {
        _mockManufacturerStatusHandler = new Mock<IManufacturerStatusHandler>();
        _manufacturerStatusController = new ManufacturerStatusController(_mockManufacturerStatusHandler.Object);
    }

    [Fact]
    public async Task GetManufacturerStatuses_ReturnsOk_WithListOfManufacturerStatuses()
    {
        var status1 = "Status1";
        var status2 = "Status2";

        var mockManufacturerStatuses = new List<ManufacturerStatusDTO>
        {
            new() { StatusName = status1 },
            new() { StatusName = status2 },
        };
        _mockManufacturerStatusHandler.Setup(handler => handler.GetManufacturerStatusesAsync())
            .ReturnsAsync(mockManufacturerStatuses);

        var result = await _manufacturerStatusController.GetManufacturerStatuses();

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnValue = Assert.IsType<List<ManufacturerStatusDTO>>(okResult.Value);

        Assert.Equal(2, returnValue.Count);
        Assert.Equal(status1, returnValue[0].StatusName);
        Assert.Equal(status2, returnValue[1].StatusName);
    }
}
