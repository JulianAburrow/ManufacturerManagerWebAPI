namespace Tests;

public class ColourControllerTests
{
    private readonly Mock<IColourHandler> _mockColourHandler;
    private readonly ColourController _colourController;

    private const string Colour1 = "Colour1";
    private const string Colour2 = "Colour2";
    private const string Colour3 = "Colour3";

    public ColourControllerTests()
    {
        _mockColourHandler = new Mock<IColourHandler>();
        _colourController = new ColourController(_mockColourHandler.Object);
    }

    [Fact]
    public async Task GetColours_ReturnsListOfColours()
    {
        var mockColours = new List<ColourDTO> { new() { Name = Colour1 }, };
        _mockColourHandler.Setup(handler => handler.GetColoursAsync())
            .ReturnsAsync(mockColours);

        var result = await _colourController.GetColours();

        var returnValue = Assert.IsType<List<ColourDTO>>(result.Value);
        Assert.Single(returnValue);
    }

    [Fact]
    public async Task GetColour_ReturnsColour()
    {
        // Arrange
        var mockColour = new ColourDTO { ColourId = 1, Name = Colour1 };
        _mockColourHandler.Setup(handler => handler.GetColourAsync(1))
            .ReturnsAsync(mockColour);

        // Act
        var result = await _colourController.GetColour(1);

        // Assert
        var returnValue = Assert.IsType<ColourDTO>(result.Value);
        Assert.Equal(1, returnValue.ColourId);
        Assert.Equal(Colour1, returnValue.Name);
    }

    [Fact]
    public async Task GetColour_ReturnsNull_WhenColourNotFound()
    {
        _mockColourHandler.Setup(handler => handler.GetColourAsync(1))
            .ReturnsAsync((ColourDTO)null);

        var result = await _colourController.GetColour(1);

        Assert.Null(result.Result);
    }

    [Fact]
    public async Task CreateColour_ReturnsOkResult_WhenCreateSuccessful()
    {
        var newColour = new ColourDTO { Name = Colour2 };
        _mockColourHandler.Setup(handler => handler.CreateColourAsync(newColour))
            .ReturnsAsync(new OkResult());

        var result = await _colourController.CreateColour(newColour);

        Assert.IsType<OkResult>(result);
    }

    [Fact]
    public async Task UpdateColour_ReturnsActionResult_WhenUpdateSuccessful()
    {
        var updatedColour = new ColourDTO { Name = Colour3 };
        _mockColourHandler.Setup(handler => handler.UpdateColourAsync(1, updatedColour))
            .ReturnsAsync(new OkResult());

        var result = await _colourController.UpdateColour(1, updatedColour);

        Assert.IsType<OkResult>(result);
    }
}
