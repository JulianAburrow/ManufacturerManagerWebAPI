using System.Net.WebSockets;

namespace Tests;

public class WidgetStatusControllerTests
{
    private readonly Mock<IWidgetStatusHandler> _mockWidgetStatusHandler;
    private readonly WidgetStatusController _widgetStatusController;

    public WidgetStatusControllerTests()
    {
        _mockWidgetStatusHandler = new Mock<IWidgetStatusHandler>();
        _widgetStatusController = new WidgetStatusController(_mockWidgetStatusHandler.Object);
    }

    [Fact]
    public async Task GetWidgetStatuses_GetsWidgetStatuses()
    {
        var status1 = "Status1";
        var status2 = "Status2";

        var mockWidgetStatuses = new List<WidgetStatusDTO>
        {
            new() { StatusName = status1 },
            new() {StatusName = status2 },
        };
        _mockWidgetStatusHandler.Setup(handler => handler.GetWidgetStatusesAsync())
            .ReturnsAsync(mockWidgetStatuses);

        var result = await _widgetStatusController.GetWidgetStatuses();
        var returnValue = Assert.IsType<List<WidgetStatusDTO>>(result);

        Assert.Equal(2, returnValue.Count);
        Assert.Equal(status1, returnValue[0].StatusName);
        Assert.Equal(status2, returnValue[1].StatusName);
    }
}
