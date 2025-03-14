namespace Tests;

public class ManufacturerControllerTests
{
    private readonly Mock<IManufacturerHandler> _mockManufacturerHandler;
    private readonly ManufacturerController _manufacturerController;

    private const string Manufacturer1 = "Manufacturer1";
    private const string Manufacturer2 = "Manufacturer2";
    private const string ActiveStatus = "Active";

    public ManufacturerControllerTests()
    {
        _mockManufacturerHandler = new Mock<IManufacturerHandler>();
        _manufacturerController = new ManufacturerController(_mockManufacturerHandler.Object);
    }

    [Fact]
    public async Task GetManufacturers_ReturnsListOfManufacturers()
    {
        var mockManufacturers = new List<ManufacturerDTO> { new() { Name = Manufacturer1 }, };
        _mockManufacturerHandler.Setup(handler => handler.GetManufacturersAsync())
            .ReturnsAsync(mockManufacturers);

        var result = await _manufacturerController.GetManufacturers();

        var returnValue = Assert.IsType<List<ManufacturerDTO>>(result.Value);
        Assert.Single(returnValue);
    }

    [Fact]
    public async Task GetManufacturer_ReturnsManufacturer()
    {
        var mockManufacturer = new ManufacturerDTO { ManufacturerId = 1, Name = Manufacturer1, StatusName = ActiveStatus };
        _mockManufacturerHandler.Setup(handler => handler.GetManufacturerAsync(1))
            .ReturnsAsync(mockManufacturer);

        var result = await _manufacturerController.GetManufacturer(1);

        var returnValue = Assert.IsType<ManufacturerDTO>(result.Value);
        Assert.Equal(1, returnValue.ManufacturerId);
        Assert.Equal(Manufacturer1, returnValue.Name);
        Assert.Equal(ActiveStatus, returnValue.StatusName);
    }

    [Fact]
    public async Task GetManufacturer_ReturnsNull_WhenManufacturerNotFound()
    {
        _mockManufacturerHandler.Setup(handler => handler.GetManufacturerAsync(1))
            .ReturnsAsync((ManufacturerDTO)null);

        var result = await _manufacturerController.GetManufacturer(1);

        Assert.Null(result.Result);
    }

    [Fact]
    public async Task CreateManufacturer_ReturnsOkResult_WhenCreateSuccessful()
    {
        var newManufacturer = new ManufacturerDTO { Name = Manufacturer1, StatusId = 1 };
        _mockManufacturerHandler.Setup(handler => handler.CreateManufacturerAsync(newManufacturer))
            .ReturnsAsync(new OkResult());

        var result = await _manufacturerController.CreateManufacturer(newManufacturer);

        Assert.IsType<OkResult>(result);
    }

    [Fact]
    public async Task UpdateManufacturer_ReturnsOkResult_WhenUpdateSuccessful()
    {
        var updatedManufacturer = new ManufacturerDTO { Name = Manufacturer2, StatusId = 2 };
        _mockManufacturerHandler.Setup(handler => handler.UpdateManufacturerAsync(1, updatedManufacturer))
            .ReturnsAsync(new OkResult());

        var result = await _manufacturerController.UpdateManufacturer(1, updatedManufacturer);

        Assert.IsType<OkResult>(result);
    }
}
