namespace Tests;

public class ColourJustificationControllerTests
{
    private readonly Mock<IColourJustificationHandler> _mockColourJustificationHandler;
    private readonly ColourJustificationController _colourJustificationController;

    private const string Justification1 = "Justification1";
    private const string Justification2 = "Justification2";
    private const string Justification3 = "Justification3";

    public ColourJustificationControllerTests()
    {
        _mockColourJustificationHandler = new Mock<IColourJustificationHandler>();
        _colourJustificationController = new ColourJustificationController(_mockColourJustificationHandler.Object);
    }

    [Fact]
    public async Task GetColourJustifications_ReturnsOk_WithListOfColourJustifications()
    {
        var mockColourJustifications = new List<ColourJustificationDTO>
        {
            new() { Justification = Justification1 }
        };
        _mockColourJustificationHandler.Setup(handler => handler.GetColourJustificationsAsync())
            .ReturnsAsync(mockColourJustifications);

        var result = await _colourJustificationController.GetColourJustifications();

        var returnValue = Assert.IsType<List<ColourJustificationDTO>>(result.Value);
        Assert.Single(returnValue);
    }

    [Fact]
    public async Task GetColourJustification_ReturnsOkResult_WithColourJustification()
    {
        var mockColourJustification = new ColourJustificationDTO { ColourJustificationId = 1, Justification = Justification1 };
        _mockColourJustificationHandler.Setup(handler => handler.GetColourJustificationAsync(1))
            .ReturnsAsync(mockColourJustification);

        var result = await _colourJustificationController.GetColourJustification(1);

        var returnValue = Assert.IsType<ColourJustificationDTO>(result.Value);
        Assert.Equal(1, returnValue.ColourJustificationId);
        Assert.Equal(Justification1, returnValue.Justification);
    }

    [Fact]
    public async Task GetColourJustification_ReturnsNull_WhenColourJustificationNotFound()
    {
        _mockColourJustificationHandler.Setup(handler => handler.GetColourJustificationAsync(1))
            .ReturnsAsync((ColourJustificationDTO)null);

        var result = await _colourJustificationController.GetColourJustification(1);

        Assert.Null(result.Result);
    }

    [Fact]
    public async Task CreateColourJustification_ReturnsOkResult_WhenCreateSuccessful()
    {
        var newColourJustification = new ColourJustificationDTO { Justification = Justification2 };
        _mockColourJustificationHandler.Setup(handler => handler.CreateColourJustificationAsync(newColourJustification))
            .ReturnsAsync(new OkResult());

        var result = await _colourJustificationController.CreateColourJustification(newColourJustification);

        Assert.IsType<OkResult>(result);
    }

    [Fact]
    public async Task UpdateColour_ReturnsActionResult_WhenUpdateSuccessful()
    {
        var updatedColourJustification = new ColourJustificationDTO { Justification = Justification3 };
        _mockColourJustificationHandler.Setup(handler => handler.UpdateColourJustificationAsync(1, updatedColourJustification))
            .ReturnsAsync(new OkResult());

        var result = await _colourJustificationController.UpdateColourJustification(1, updatedColourJustification);

        Assert.IsType<OkResult>(result);
    }
}
