namespace Tests;

public class WidgetHandlerTests
{
    private readonly WidgetHandler _handler;
    private readonly ManufacturerManagerDbContext _context;

    private const string WidgetName1 = "WidgetName1";
    private const string WidgetName2 = "WidgetName2";

    private const string ActiveStatus = "Active";
    private const string InactiveStatus = "Inactive";

    private WidgetModel WidgetModel1 = new()
    {
        Name = WidgetName1,
        ManufacturerId = 1,
        Manufacturer = new ManufacturerModel { Name = "Manufacturer1" },
        StatusId = 1,
        Status = new WidgetStatusModel { StatusId = 1, StatusName = "Active" },
        ColourId = 1,
        ColourJustificationId = 1,
        CostPrice = 1,
        RetailPrice = 1,
        StockLevel = 1,
    };

    private WidgetModel WidgetModel2 = new()
    {
        Name = WidgetName2,
        ManufacturerId = 2,
        Manufacturer = new ManufacturerModel { Name = "Manufacturer2" },
        StatusId = 2,
        Status = new WidgetStatusModel { StatusId = 2, StatusName = "Inactive" },
        ColourId = 2,
        ColourJustificationId = 2,
        CostPrice = 1,
        RetailPrice = 1,
        StockLevel = 1,
    };

    private WidgetDTO WidgetDTO1 = new()
    {
        Name = WidgetName1,
        ManufacturerId = 1,
        StatusId = 1,
        CostPrice = 1,
        RetailPrice = 1,
        StockLevel = 1
    };

    private WidgetDTO WidgetDTO2 = new()
    {
        Name = WidgetName2,
        ManufacturerId = 1,
        StatusId = 2,
        CostPrice = 1,
        RetailPrice = 1,
        StockLevel = 1
    };

    public WidgetHandlerTests()
    {
        _context = Shared.GetInMemoryDbContext();
        _handler = new WidgetHandler(_context);
    }


    [Fact]
    public async Task GetWidgets_GetsAllWidgets()
    {
        await RemoveAllWidgetsFromContext();

        _context.Widgets.AddRange(new List<WidgetModel>
        {
            WidgetModel1,
            WidgetModel2,
        });
        await _context.SaveChangesAsync();

        var result = await _handler.GetWidgetsAsync();
        Assert.Equal(2, result.Count);
        Assert.Equal(WidgetName1, result[0].Name);
        Assert.Equal(1, result[0].StatusId);
        Assert.Equal(WidgetName2, result[1].Name);
        Assert.Equal(2, result[1].StatusId);
    }

    [Fact]
    public async Task GetWidget_GetsCorrectWidget()
    {
        await RemoveAllWidgetsFromContext();

        _context.Widgets.AddRange(new List<WidgetModel>
        {
            WidgetModel1,
            WidgetModel2,
        });
        await _context.SaveChangesAsync();

        var result = await _handler.GetWidgetAsync(1);

        Assert.NotNull(result);
        Assert.Equal(WidgetName1, result.Name);
        Assert.Equal(1, result.ManufacturerId);
        Assert.Equal("Manufacturer1", result.ManufacturerName);
        Assert.Equal(1, result.CostPrice);
        Assert.Equal(1, result.RetailPrice);
        Assert.Equal(1, result.StockLevel);
        Assert.Equal(1, result.StatusId);
        Assert.Equal("Active", result.StatusName);
    }

    [Fact]
    public async Task GetWidget_ReturnsNull_WhenWidgetDoesNotExist()
    {
        var result = await _handler.GetWidgetAsync(999);

        Assert.Null(result);
    }

    [Fact]
    public async Task GetWidgetsForManufacturer_ReturnsCorrectWidgetsForManufacturer()
    {
        await RemoveAllWidgetsFromContext();
        _context.Manufacturers.RemoveRange(_context.Manufacturers);
        _context.WidgetStatuses.RemoveRange(_context.WidgetStatuses);
        await _context.SaveChangesAsync();

        _context.Widgets.AddRange(new List<WidgetModel>
        {
            WidgetModel1,
            WidgetModel2,
        });

        await _context.SaveChangesAsync();

        var result = await _handler.GetWidgetsForManufacturerAsync(1);
        var actualWidgets = Assert.IsType<List<WidgetDTO>>(result);
        Assert.Single(actualWidgets);
        Assert.Equal(WidgetName1, actualWidgets[0].Name);
        Assert.Equal(1, actualWidgets[0].WidgetId);
    }

    [Fact]
    public async Task GetWidgetsForColour_ReturnsCorrectWidgetsForColour()
    {
        await RemoveAllWidgetsFromContext();
        _context.Manufacturers.RemoveRange(_context.Manufacturers);
        _context.WidgetStatuses.RemoveRange(_context.WidgetStatuses);
        await _context.SaveChangesAsync();

        _context.Widgets.AddRange(new List<WidgetModel>
        {
            WidgetModel1,
            WidgetModel2,
        });
        await _context.SaveChangesAsync();

        var result = await _handler.GetWidgetsForColourAsync(1);
        var actualWidgets = Assert.IsType<List<WidgetDTO>>(result);
        Assert.Single(actualWidgets);
        Assert.Equal(WidgetName1, actualWidgets[0].Name);
        Assert.Equal(1, actualWidgets[0].WidgetId);
    }

    [Fact]
    public async Task GetWidgetsForColourJustification_ReturnsCorrectWidgetsForColourJustification()
    {
        await RemoveAllWidgetsFromContext();
        _context.Manufacturers.RemoveRange(_context.Manufacturers);
        _context.WidgetStatuses.RemoveRange(_context.WidgetStatuses);
        await _context.SaveChangesAsync();

        _context.Widgets.AddRange(new List<WidgetModel>
        {
            WidgetModel1,
            WidgetModel2,
        });
        await _context.SaveChangesAsync();

        var result = await _handler.GetWidgetsForColourJustificationAsync(1);
        var actualWidgets = Assert.IsType<List<WidgetDTO>>(result);
        Assert.Single(actualWidgets);
        Assert.Equal(WidgetName1, actualWidgets[0].Name);
        Assert.Equal(1, actualWidgets[0].WidgetId);
    }

    [Fact]
    public async Task CreateWidget_CreatesWidget()
    {
        await RemoveAllWidgetsFromContext();

        var widgetDTO = new WidgetDTO
        {
            Name = WidgetName1,
            ManufacturerId = 1,
            StatusId = 1,
            ColourId = 1,
            ColourJustificationId = 1,
            CostPrice = 1,
            RetailPrice = 1,
            StockLevel = 1
        };

        var result = await _handler.CreateWidgetAsync(widgetDTO);

        var createdResult = Assert.IsType<CreatedResult>(result);
        var createdWidget = await _context.Widgets
            .FirstOrDefaultAsync(w => w.Name == WidgetName1);
        Assert.NotNull(createdWidget);
        Assert.Equal(WidgetName1, createdWidget.Name);
        Assert.Equal(1, createdWidget.ManufacturerId);
        Assert.Equal(1, createdWidget.StatusId);
        Assert.Equal(1, createdWidget.ColourId);
        Assert.Equal(1, createdWidget.ColourJustificationId);
        Assert.Equal(1, createdWidget.CostPrice);
        Assert.Equal(1, createdWidget.RetailPrice);
        Assert.Equal(1, createdWidget.StockLevel);
    }

    [Fact]
    public async Task UpdateWidget_ReturnsNotFound_WhenWidgetDoesNotExist()
    {
        var result = await _handler.UpdateWidgetAsync(999, WidgetDTO1);

        Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public async Task UpdateWidget_ReturnsOk_WhenWidgetSuccessfullyUpdated()
    {
        await RemoveAllWidgetsFromContext();

        _context.Widgets.Add(WidgetModel1);
        await _context.SaveChangesAsync();

        var createdWidget = await _context.Widgets
            .FirstOrDefaultAsync(w => w.Name == WidgetModel1.Name);
        Assert.NotNull(createdWidget);

        var result = await _handler.UpdateWidgetAsync(createdWidget.WidgetId,
            new WidgetDTO
            {
                Name = "UpdatedWidgetName",
                ManufacturerId = 1,
                StatusId = 1,
                ColourId = 1,
                ColourJustificationId = 1,
                CostPrice = 1,
                RetailPrice = 1,
                StockLevel = 1,
            });

        Assert.IsType<OkResult>(result);
    }

    private async Task RemoveAllWidgetsFromContext()
    {
        _context.RemoveRange(_context.Widgets);
        await _context.SaveChangesAsync();
    }
}
