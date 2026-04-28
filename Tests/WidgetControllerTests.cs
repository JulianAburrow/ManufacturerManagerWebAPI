namespace Tests;

public class WidgetControllerTests
{
    private readonly Mock<IWidgetHandler> _mockWidgetHandler;
    private readonly WidgetController _widgetController;

    private const string Widget1 = "Widget1";
    private const string Widget2 = "Widget2";

    public WidgetControllerTests()
    {
        _mockWidgetHandler = new Mock<IWidgetHandler>();
        _widgetController = new WidgetController(_mockWidgetHandler.Object);
    }

    [Fact]
    public async Task GetWidgets_ReturnsOk_WithListOfWidgets()
    {
        var mockWidgets = new List<WidgetDTO> { new() { Name = Widget1 } };
        _mockWidgetHandler.Setup(handler => handler.GetWidgetsAsync())
            .ReturnsAsync(mockWidgets);

        var result = await _widgetController.GetWidgets();

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnValue = Assert.IsType<List<WidgetDTO>>(okResult.Value);

        Assert.Single(returnValue);
        Assert.Equal(Widget1, returnValue[0].Name);
    }

    [Fact]
    public async Task GetWidget_ReturnsOk_WithWidget()
    {
        var mockWidget = new WidgetDTO { WidgetId = 1, Name = Widget1, StatusId = 1 };
        _mockWidgetHandler.Setup(handler => handler.GetWidgetAsync(1))
            .ReturnsAsync((WidgetDTO?)mockWidget);


        var result = await _widgetController.GetWidget(1);

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnValue = Assert.IsType<WidgetDTO>(okResult.Value);

        Assert.Equal(1, returnValue.WidgetId);
        Assert.Equal(Widget1, returnValue.Name);
        Assert.Equal(1, returnValue.StatusId);
    }

    [Fact]
    public async Task GetWidget_ReturnsNotFound_WhenWidgetNotFound()
    {
        _mockWidgetHandler.Setup(handler => handler.GetWidgetAsync(1))
            .ReturnsAsync((WidgetDTO?)null);

        var result = await _widgetController.GetWidget(1);

        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task CreateWidget_ReturnsOkResult_WhenCreateSuccessful()
    {
        var newWidget = new WidgetDTO { Name = Widget1, ManufacturerId = 1, StatusId = 1 };
        _mockWidgetHandler.Setup(handler => handler.CreateWidgetAsync(newWidget))
            .ReturnsAsync(new OkResult());

        var result = await _widgetController.CreateWidget(newWidget);

        Assert.IsType<OkResult>(result);
    }

    [Fact]
    public async Task UpdateWidget_ReturnsOkResult_WhenUpdateSuccessful()
    {
        var updatedWidget = new WidgetDTO { Name = Widget2, ManufacturerId = 1, StatusId = 1 };
        _mockWidgetHandler.Setup(handler => handler.UpdateWidgetAsync(1, updatedWidget))
            .ReturnsAsync(new OkResult());

        var result = await _widgetController.UpdateWidget(1, updatedWidget);

        Assert.IsType<OkResult>(result);
    } 
}
