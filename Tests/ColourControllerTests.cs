namespace Tests;

public class ColourControllerTests
{
    private readonly Mock<IColourHandler> _mockColourHandler;
    private readonly Mock<IWidgetHandler> _mockWidgetHandler;
    private readonly ColourController _colourController;

    private const string Colour1 = "Colour1";
    private const string Colour2 = "Colour2";
    private const string Colour3 = "Colour3";
    private const string Widget1 = "Widget1";
    private const string Widget2 = "Widget2";

    public ColourControllerTests()
    {
        _mockColourHandler = new Mock<IColourHandler>();
        _mockWidgetHandler = new Mock<IWidgetHandler>();
        _colourController = new ColourController(_mockColourHandler.Object, _mockWidgetHandler.Object);
    }

    [Fact]
    public async Task GetColours_ReturnsOk_WithListOfColours()
    {
        var mockColours = new List<ColourDTO> { new() { Name = Colour1 } };
        _mockColourHandler.Setup(h => h.GetColoursAsync())
            .ReturnsAsync(mockColours);

        var result = await _colourController.GetColours();

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnValue = Assert.IsType<List<ColourDTO>>(okResult.Value);

        Assert.Single(returnValue);
    }

    [Fact]
    public async Task GetColour_ReturnsOk_WithColour()
    {
        // Arrange
        var mockColour = new ColourDTO { ColourId = 1, Name = Colour1 };
        _mockColourHandler.Setup(handler => handler.GetColourAsync(1))
            .ReturnsAsync(mockColour);

        // Act
        var result = await _colourController.GetColour(1);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnValue = Assert.IsType<ColourDTO>(okResult.Value);

        Assert.Equal(1, returnValue.ColourId);
        Assert.Equal(Colour1, returnValue.Name);
    }

    [Fact]
    public async Task GetColour_ReturnsNotFound_WhenColourNotFound()
    {
        _mockColourHandler.Setup(handler => handler.GetColourAsync(1))
            .ReturnsAsync((ColourDTO?)null);

        var result = await _colourController.GetColour(1);

        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task CreateColour_ReturnsCreatedResult_WhenCreateSuccessful()
    {
        var newColour = new ColourDTO { Name = Colour2 };
        _mockColourHandler.Setup(handler => handler.CreateColourAsync(newColour))
            .ReturnsAsync(new CreatedResult("/api/colours/1", newColour));

        var result = await _colourController.CreateColour(newColour);

        Assert.IsType<CreatedResult>(result);
    }

    [Fact]
    public async Task UpdateColour_ReturnsOk_WhenUpdateSuccessful()
    {
        var updatedColour = new ColourDTO { Name = Colour3 };
        _mockColourHandler.Setup(handler => handler.UpdateColourAsync(1, updatedColour))
            .ReturnsAsync(new OkResult());

        var result = await _colourController.UpdateColour(1, updatedColour);

        Assert.IsType<OkResult>(result);
    }

    [Fact]
    public async Task GetWidgetsForColour_ReturnsOk_WithFilteredWidgets()
    {
        var mockWidgets = new List<WidgetDTO>
        {
            new() { WidgetId = 1, Name = Widget1, ManufacturerId = 1, ColourId = 1 },
        };
        _mockWidgetHandler.Setup(h => h.GetWidgetsForColourAsync(1))
            .ReturnsAsync(mockWidgets);

        var result = await _colourController.GetWidgetsForColour(1);

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnValue = Assert.IsType<List<WidgetDTO>>(okResult.Value);

        Assert.Single(returnValue);
        Assert.Equal(1, returnValue[0].WidgetId);
        Assert.Equal(Widget1, returnValue[0].Name);
    }
}
