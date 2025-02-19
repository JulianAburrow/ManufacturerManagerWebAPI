using WebAPI.Data;

namespace Tests;

public class WidgetTests
{
    private const string TestWidget = "TestWidget";
    private const string Widget1 = "Widget1";
    private const string Widget2 = "Widget2";
    private const string Widget3 = "Widget3";
    private const string Widget4 = "Widget4";

    [Fact]
    public async Task GetWidgets_GetsAllWidgets()
    {
        var context = Shared.GetInMemoryDbContext();
        context.Widgets.RemoveRange(context.Widgets);
        context.Manufacturers.RemoveRange(context.Manufacturers);
        context.WidgetStatuses.RemoveRange(context.WidgetStatuses);

        context.Widgets.AddRange(new List<WidgetModel>
        {
            new() { Name = Widget1, ManufacturerId = 1, Manufacturer = new ManufacturerModel { Name = "Manufacturer1" }, StatusId = 1, Status = new WidgetStatusModel { StatusId = 1, StatusName = "Active" }, CostPrice = 1, RetailPrice = 1, StockLevel = 1 },
            new() { Name = Widget2, ManufacturerId = 1, Manufacturer = new ManufacturerModel { Name = "Manufacturer1" }, StatusId = 1, Status = new WidgetStatusModel { StatusId = 2, StatusName = "Active" }, CostPrice = 1, RetailPrice = 1, StockLevel = 1 },
            new() { Name = Widget3, ManufacturerId = 1, Manufacturer = new ManufacturerModel { Name = "Manufacturer1" }, StatusId = 1, Status = new WidgetStatusModel { StatusId = 3, StatusName = "Active" }, CostPrice = 1, RetailPrice = 1, StockLevel = 1 },
            new() { Name = Widget4, ManufacturerId = 1, Manufacturer = new ManufacturerModel { Name = "Manufacturer1" }, StatusId = 1, Status = new WidgetStatusModel { StatusId = 4, StatusName = "Active" }, CostPrice = 1, RetailPrice = 1, StockLevel = 1 },
        });

        await context.SaveChangesAsync();

        var controller = new WidgetController(context);

        var result = await controller.GetWidgets();
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var actualWidgets = Assert.IsType<List<WidgetDTO>>(okResult.Value);
        Assert.Equal(4, actualWidgets.Count);
        Assert.Equal(Widget1, actualWidgets[0].Name);
        Assert.Equal(Widget2, actualWidgets[1].Name);
        Assert.Equal(Widget3, actualWidgets[2].Name);
        Assert.Equal(Widget4, actualWidgets[3].Name);
    }

    [Fact]
    public async Task GetWidget_GetsCorrectWidget()
    {
        var context = Shared.GetInMemoryDbContext();
        context.Widgets.RemoveRange(context.Widgets);
        await context.SaveChangesAsync();

        var controller = new WidgetController(context);
        context.Widgets.Add(new WidgetModel { Name = "Widget1", ManufacturerId = 1, Manufacturer = new ManufacturerModel { Name = "Manufacturer1" }, StatusId = 1, Status = new WidgetStatusModel { StatusId = 1, StatusName = "Active" }, CostPrice = 1, RetailPrice = 1, StockLevel = 1 });
        await context.SaveChangesAsync();

        var result = await controller.GetWidget(1);
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var actualWidget = Assert.IsType<WidgetDTO>(okResult.Value);
        Assert.Equal("Widget1", actualWidget.Name);
        Assert.Equal(1, actualWidget.ManufacturerId);
        Assert.Equal("Manufacturer1", actualWidget.ManufacturerName);
        Assert.Equal(1, actualWidget.CostPrice);
        Assert.Equal(1, actualWidget.RetailPrice);
        Assert.Equal(1, actualWidget.StockLevel);
        Assert.Equal(1, actualWidget.StatusId);
        Assert.Equal("Active", actualWidget.StatusName);
    }

    [Fact]
    public async Task GetWidget_ReturnsNotFound_WhenWidgetDoesNotExist()
    {
        var context = Shared.GetInMemoryDbContext();
        var controller = new WidgetController(context);

        var result = await controller.GetWidget(999);

        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task GetWidgetsForManufacture_ReturnsCorrectWidgetsForManufacturer()
    {
        var context = Shared.GetInMemoryDbContext();
        context.Widgets.RemoveRange(context.Widgets);
        context.Manufacturers.RemoveRange(context.Manufacturers);
        context.WidgetStatuses.RemoveRange(context.WidgetStatuses);
        await context.SaveChangesAsync();

        context.Widgets.AddRange(new List<WidgetModel>
        {
            new() { Name = Widget1, ManufacturerId = 1, Manufacturer = new ManufacturerModel { Name = "Manufacturer1" }, StatusId = 1, Status = new WidgetStatusModel { StatusId = 1, StatusName = "Active" }, ColourId = 1, CostPrice = 1, RetailPrice = 1, StockLevel = 1 },
            new() { Name = Widget2, ManufacturerId = 2, Manufacturer = new ManufacturerModel { Name = "Manufacturer1" }, StatusId = 1, Status = new WidgetStatusModel { StatusId = 2, StatusName = "Active" }, ColourId = 2, CostPrice = 1, RetailPrice = 1, StockLevel = 1 },
            new() { Name = Widget3, ManufacturerId = 3, Manufacturer = new ManufacturerModel { Name = "Manufacturer1" }, StatusId = 1, Status = new WidgetStatusModel { StatusId = 3, StatusName = "Active" }, ColourId = 3, CostPrice = 1, RetailPrice = 1, StockLevel = 1 },
            new() { Name = Widget4, ManufacturerId = 4, Manufacturer = new ManufacturerModel { Name = "Manufacturer1" }, StatusId = 1, Status = new WidgetStatusModel { StatusId = 4, StatusName = "Active" }, ColourId = 4, CostPrice = 1, RetailPrice = 1, StockLevel = 1 },
        });

        await context.SaveChangesAsync();

        var controller = new WidgetController(context);

        var result = await controller.GetWidgetsForManufacturer(1);
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var actualWidgets = Assert.IsType<List<WidgetDTO>>(okResult.Value);
        Assert.Single(actualWidgets);
    }

    [Fact]
    public async Task GetWidgetsForColour_ReturnsCorrectWidgetsForColour()
    {
        var context = Shared.GetInMemoryDbContext();
        context.Widgets.RemoveRange(context.Widgets);
        context.Manufacturers.RemoveRange(context.Manufacturers);
        context.WidgetStatuses.RemoveRange(context.WidgetStatuses);
        await context.SaveChangesAsync();

        context.Widgets.AddRange(new List<WidgetModel>
        {
            new() { Name = Widget1, ManufacturerId = 1, Manufacturer = new ManufacturerModel { Name = "Manufacturer1" }, StatusId = 1, Status = new WidgetStatusModel { StatusId = 1, StatusName = "Active" }, ColourId = 1, CostPrice = 1, RetailPrice = 1, StockLevel = 1 },
            new() { Name = Widget2, ManufacturerId = 2, Manufacturer = new ManufacturerModel { Name = "Manufacturer1" }, StatusId = 1, Status = new WidgetStatusModel { StatusId = 2, StatusName = "Active" }, ColourId = 2, CostPrice = 1, RetailPrice = 1, StockLevel = 1 },
            new() { Name = Widget3, ManufacturerId = 3, Manufacturer = new ManufacturerModel { Name = "Manufacturer1" }, StatusId = 1, Status = new WidgetStatusModel { StatusId = 3, StatusName = "Active" }, ColourId = 3, CostPrice = 1, RetailPrice = 1, StockLevel = 1 },
            new() { Name = Widget4, ManufacturerId = 4, Manufacturer = new ManufacturerModel { Name = "Manufacturer1" }, StatusId = 1, Status = new WidgetStatusModel { StatusId = 4, StatusName = "Active" }, ColourId = 4, CostPrice = 1, RetailPrice = 1, StockLevel = 1 },
        });

        await context.SaveChangesAsync();

        var controller = new WidgetController(context);

        var result = await controller.GetWidgetsForColour(1);
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var actualWidgets = Assert.IsType<List<WidgetDTO>>(okResult.Value);
        Assert.Single(actualWidgets);
    }

    [Fact]
    public async Task GetWidgetsForColourJustification_ReturnsCorrectWidgetsForColourJustification()
    {
        var context = Shared.GetInMemoryDbContext();
        context.Widgets.RemoveRange(context.Widgets);
        await context.SaveChangesAsync();

        context.Widgets.AddRange(new List<WidgetModel>
        {
            new() { Name = Widget1, ManufacturerId = 1, Manufacturer = new ManufacturerModel { Name = "Manufacturer1" }, StatusId = 1, Status = new WidgetStatusModel { StatusId = 1, StatusName = "Active" }, ColourId = 1, ColourJustificationId = 1, CostPrice = 1, RetailPrice = 1, StockLevel = 1 },
            new() { Name = Widget2, ManufacturerId = 2, Manufacturer = new ManufacturerModel { Name = "Manufacturer1" }, StatusId = 1, Status = new WidgetStatusModel { StatusId = 2, StatusName = "Active" }, ColourId = 2, ColourJustificationId = 2, CostPrice = 1, RetailPrice = 1, StockLevel = 1 },
            new() { Name = Widget3, ManufacturerId = 3, Manufacturer = new ManufacturerModel { Name = "Manufacturer1" }, StatusId = 1, Status = new WidgetStatusModel { StatusId = 3, StatusName = "Active" }, ColourId = 3, ColourJustificationId = 3, CostPrice = 1, RetailPrice = 1, StockLevel = 1 },
            new() { Name = Widget4, ManufacturerId = 4, Manufacturer = new ManufacturerModel { Name = "Manufacturer1" }, StatusId = 1, Status = new WidgetStatusModel { StatusId = 4, StatusName = "Active" }, ColourId = 4, ColourJustificationId = 4, CostPrice = 1, RetailPrice = 1, StockLevel = 1 },
        });

        await context.SaveChangesAsync();

        var controller = new WidgetController(context);

        var result = await controller.GetWidgetsForColourJustification(1);
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var actualWidgets = Assert.IsType<List<WidgetDTO>>(okResult.Value);
        Assert.Single(actualWidgets);
    }

    [Fact]
    public async Task CreateWidget_CreatesWidget()
    {
        var context = Shared.GetInMemoryDbContext();
        context.Widgets.RemoveRange(context.Widgets);
        await context.SaveChangesAsync();

        var widgetDTO = new WidgetDTO
        {
            Name = TestWidget,
            ManufacturerId = 1,
            StatusId = 1,
            CostPrice = 1,
            RetailPrice = 1,
            StockLevel = 1
        };

        var controller = new WidgetController(context);

        var result = await controller.CreateWidget(widgetDTO);

        var createdResult = Assert.IsType<CreatedResult>(result);
        var createdWidget = await context.Widgets
            .FirstOrDefaultAsync(w => w.Name == TestWidget);
        Assert.NotNull(createdWidget);
        Assert.Equal(TestWidget, createdWidget.Name);
        Assert.Equal(1, createdWidget.ManufacturerId);
        Assert.Equal(1, createdWidget.StatusId);
        Assert.Equal(1, createdWidget.CostPrice);
        Assert.Equal(1, createdWidget.RetailPrice);
        Assert.Equal(1, createdWidget.StockLevel);
    }
}
