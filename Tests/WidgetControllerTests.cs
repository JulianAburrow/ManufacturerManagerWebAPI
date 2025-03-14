using Microsoft.Identity.Client;
using WebAPI.Controllers;

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
    public async Task GetWidgets_ReturnsListOfWidgets()
    {
        var mockWidgets = new List<WidgetDTO> { new() { Name = Widget1 } };
        _mockWidgetHandler.Setup(handler => handler.GetWidgetsAsync())
            .ReturnsAsync(mockWidgets);

        var result = await _widgetController.GetWidgets();

        var returnValue = Assert.IsType<List<WidgetDTO>>(result.Value);
        Assert.Single(returnValue);
    }

    [Fact]
    public async Task GetWidget_ReturnsWidget()
    {
        var mockWidget = new WidgetDTO { WidgetId = 1, Name = Widget1, StatusId = 1 };
        _mockWidgetHandler.Setup(handler => handler.GetWidgetAsync(1))
            .ReturnsAsync(mockWidget);

        var result = await _widgetController.GetWidget(1);

        var returnValue = Assert.IsType<WidgetDTO>(result.Value);
        Assert.Equal(1, returnValue.WidgetId);
        Assert.Equal(Widget1, returnValue.Name);
        Assert.Equal(1, returnValue.StatusId);
    }

    [Fact]
    public async Task GetWidget_ReturnsNull_WhenWidgetNotFound()
    {
        _mockWidgetHandler.Setup(handler => handler.GetWidgetAsync(1))
            .ReturnsAsync((WidgetDTO)null);

        var result = await _widgetController.GetWidget(1);

        Assert.Null(result.Result);
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

    [Fact]
    public async Task CheckForExistingWidgets_ReturnsOk_WhenNoExistingWidgets()
    {
        _mockWidgetHandler.Setup(handler => handler.CheckForExistingWidgetAsync("WidgetName", 1))
            .ReturnsAsync([]);

        var result = await _widgetController.CheckForExistingWidgets("WidgetName", 1);

        Assert.IsType<OkResult>(result.Result);
    }

    [Fact]
    public async Task CheckForExistingWidgets_ReturnsConflict_WhenExistingWidgetsFound()
    {
        var existingWidgets = new List<WidgetModel> { new() { WidgetId = 1, Name = Widget2 } };
        _mockWidgetHandler.Setup(handler => handler.CheckForExistingWidgetAsync(Widget2, 1))
            .ReturnsAsync(existingWidgets);

        var result = await _widgetController.CheckForExistingWidgets(Widget2, 1);

        Assert.IsType<ConflictResult>(result.Result);
    }

    //[Fact]
    //public async Task GetWidgetsForManufacturer_ReturnsListOfWidgetsForManufacturer()
    //{
    //    var mockWidgets = new List<WidgetDTO>
    //    {
    //        new() { WidgetId = 1, Name = Widget1, ManufacturerId = 1 },
    //        new() { WidgetId = 2, Name = Widget2, ManufacturerId = 2 }
    //    };
    //    _mockWidgetHandler.Setup(handler => handler.GetWidgetsForManufacturerAsync(1))
    //        .ReturnsAsync(mockWidgets);

    //    var result = await _widgetController.GetWidgetsForManufacturer(1);

    //    var returnValue = Assert.IsType<List<WidgetDTO>>(result.Value);
    //    Assert.Single(returnValue);
    //    Assert.Equal(1, returnValue[0].WidgetId);
    //    Assert.Equal(Widget1, returnValue[0].Name);
    //}

    //[Fact]
    //public async Task GetWidgetsForColour_ReturnsListOfWidgetsForColour()
    //{
    //    var mockWidgets = new List<WidgetDTO>
    //    {
    //        new() { WidgetId = 1, Name = Widget1, ManufacturerId = 1, ColourId = 1 },
    //        new() { WidgetId = 2, Name = Widget2, ManufacturerId = 1, ColourId = 2 },
    //    };
    //    _mockWidgetHandler.Setup(handler => handler.GetWidgetsForColourAsync(1))
    //        .ReturnsAsync(mockWidgets);

    //    var result = await _widgetController.GetWidgetsForColour(1);

    //    var returnValue = Assert.IsType<List<WidgetDTO>>(result.Value);
    //    Assert.Single(returnValue);
    //    Assert.Equal(1, returnValue[0].WidgetId);
    //    Assert.Equal(Widget1, returnValue[0].Name);
    //}

    //[Fact]
    //public async Task GetWidgetsForColour_ReturnsListOfWidgetsForColour()
    //{
    //    var mockWidgets = new List<WidgetDTO>
    //        {
    //            new() { WidgetId = 1, Name = Widget1, ColourId = 1 },
    //            new() { WidgetId = 2, Name = Widget2, ColourId = 2 }
    //        };
    //    _mockWidgetHandler.Setup(handler => handler.GetWidgetsForColourAsync(1))
    //        .ReturnsAsync(mockWidgets);

    //    var result = await _widgetController.GetWidgetsForColour(1);

    //    var returnValue = Assert.IsType<List<WidgetDTO>>(result.Value);
    //    Assert.Single(returnValue);
    //    Assert.Equal(1, returnValue[0].ColourId);
    //    //Assert.Equal(1, returnValue[1].ColourId);
    //}
}
