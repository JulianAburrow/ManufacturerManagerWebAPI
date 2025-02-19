using System.Net;

namespace Tests;

public class ManufacturerTests
{
    private const string TestManufacturer = "TestManufacturer";
    private const string ActiveStatus = "Active";
    private const string InactiveStatus = "Inactive";

    [Fact]
    public async Task GetManufacturers_GetsAllManufacturers()
    {
        var manufacturer1 = "Manufacturer1";
        var manufacturer2 = "Manufacturer2";
        var manufacturer3 = "Manufacturer3";
        var manufacturer4 = "Manufacturer4";

        var context = Shared.GetInMemoryDbContext();
        context.Manufacturers.RemoveRange(context.Manufacturers);
        await context.SaveChangesAsync();

        context.Manufacturers.AddRange(new List<ManufacturerModel>
        {
            new() { Name = manufacturer1, Status = new ManufacturerStatusModel { StatusName = ActiveStatus }, StatusId = 1 },
            new() { Name = manufacturer2, Status = new ManufacturerStatusModel { StatusName = InactiveStatus }, StatusId = 2 },
            new() { Name = manufacturer3, Status = new ManufacturerStatusModel { StatusName = ActiveStatus }, StatusId = 1 },
            new() { Name = manufacturer4, Status = new ManufacturerStatusModel { StatusName = InactiveStatus }, StatusId = 2 },
        });
        await context.SaveChangesAsync();

        var controller = new ManufacturerController(context);

        var result = await controller.GetManufacturers();

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var actualManufacturers = Assert.IsType<List<ManufacturerDTO>>(okResult.Value);
        Assert.Equal(4, actualManufacturers.Count);
        Assert.Equal(manufacturer1, actualManufacturers[0].Name);
        Assert.Equal(manufacturer2, actualManufacturers[1].Name);
        Assert.Equal(manufacturer3, actualManufacturers[2].Name);
        Assert.Equal(manufacturer4, actualManufacturers[3].Name);
    }

    [Fact]
    public async Task GetManufacturer_GetsCorrectManufacturer()
    {
        var context = Shared.GetInMemoryDbContext();
        context.Manufacturers.RemoveRange(context.Manufacturers);
        await context.SaveChangesAsync();

        var controller = new ManufacturerController(context);
        context.Manufacturers.Add(new ManufacturerModel { Name = TestManufacturer, Status = new ManufacturerStatusModel { StatusName = ActiveStatus }, StatusId = 1 });
        await context.SaveChangesAsync();

        var result = await controller.GetManufacturer(1);
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var actualManufacturer = Assert.IsType<ManufacturerDTO>(okResult.Value);
        Assert.Equal(TestManufacturer, actualManufacturer.Name);
        Assert.Equal(1, actualManufacturer.ManufacturerId);
        Assert.Equal(ActiveStatus, actualManufacturer.StatusName);
    }

    [Fact]
    public async Task GetManufacturer_ReturnsNotFound_WhenManufacturerDoesNotExist()
    {
        var context = Shared.GetInMemoryDbContext();
        var controller = new ManufacturerController(context);

        var result = await controller.GetManufacturer(999);

        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task CreateManufacturer_CreatesManufacturer()
    {
        var context = Shared.GetInMemoryDbContext();

        var manufacturerDTO = new ManufacturerDTO
        {
            Name = TestManufacturer,
            StatusId = 1,
        };

        var controller = new ManufacturerController(context);

        var result = await controller.CreateManufacturer(manufacturerDTO);

        var createdResult = Assert.IsType<CreatedResult>(result);
        var createdManufacturer = await context.Manufacturers
            .FirstOrDefaultAsync(m => m.Name == TestManufacturer);
        Assert.NotNull(createdManufacturer);
        Assert.Equal(TestManufacturer, createdManufacturer.Name);
    }

    [Fact]
    public async Task UpdateManufacturer_UpdatesManufacturer()
    {
        var newManufacturer = "NewManufacturer";
        var context = Shared.GetInMemoryDbContext();
        var controller = new ManufacturerController(context);

        context.Manufacturers.Add(new ManufacturerModel { Name = TestManufacturer, StatusId = 1 });
        await context.SaveChangesAsync();

        var createdManufacturer = await context.Manufacturers
            .FirstOrDefaultAsync(m => m.Name == TestManufacturer);
        Assert.NotNull(createdManufacturer);

        await controller.UpdateManufacturer(createdManufacturer.ManufacturerId, new ManufacturerDTO { Name = newManufacturer, StatusId = 2 });

        var updatedManufacturer = await context.Manufacturers
            .FirstOrDefaultAsync(m => m.ManufacturerId == createdManufacturer.ManufacturerId);
        Assert.NotNull(updatedManufacturer);

        Assert.Equal(newManufacturer, updatedManufacturer.Name);
        Assert.Equal(2, updatedManufacturer.StatusId);
    }

    [Fact]
    public async Task UpdateManufacturer_Returns_NotFound_WhenManufacturerDoesNotExist()
    {
        var context = Shared.GetInMemoryDbContext();
        var controller = new ManufacturerController(context);

        var result = await controller.UpdateManufacturer(999, new ManufacturerDTO { Name = TestManufacturer, StatusId = 2 });
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task CheckForExistingManufacturer_ReturnsOk_WhenManufacturerNameDoesNotExist_AndManufacturerIdIsZero()
    {
        var context = Shared.GetInMemoryDbContext();
        var controller = new ManufacturerController(context);

        var result = await controller.CheckForExistingManufacturer("SomeManufacturer", 0);
        Assert.IsType<ActionResult<HttpStatusCode>>(result);
        Assert.IsType<OkResult>(result.Result);
    }

    [Fact]
    public async Task CheckForExistingManufacturer_ReturnsConflict_WhenManufacturerNameExists_AndManufacturerIdIsZero()
    {
        var someManufacturer = "SomeManufacturer";
        var context = Shared.GetInMemoryDbContext();
        var controller = new ManufacturerController(context);
        context.Manufacturers.Add(new ManufacturerModel { Name = someManufacturer, StatusId = 1 });
        await context.SaveChangesAsync();

        var result = await controller.CheckForExistingManufacturer(someManufacturer, 0);
        Assert.IsType<ActionResult<HttpStatusCode>>(result);
        Assert.IsType<ConflictResult>(result.Result);
    }

    [Fact]
    public async Task CheckForExistingManufacturer_ReturnsOk_WhenManufacturerNameDoesNotExist_AndManufacturerIdIsGreaterThanZero()
    {
        var manufacturer1 = "Manufacturer1";
        var manufacturer2 = "Manufacturer2";
        var context = Shared.GetInMemoryDbContext();
        var controller = new ManufacturerController(context);
        
        context.Manufacturers.RemoveRange(context.Manufacturers);
        await context.SaveChangesAsync();

        context.Manufacturers.AddRange(new List<ManufacturerModel>
        {
            new() { Name = manufacturer1, StatusId = 1 },
            new() { Name = manufacturer2, StatusId = 2 },
        });
        await context.SaveChangesAsync();

        var result = await controller.CheckForExistingManufacturer(manufacturer1, 1);
        Assert.IsType<ActionResult<HttpStatusCode>>(result);
        Assert.IsType<OkResult>(result.Result);
    }

    [Fact]
    public async Task CheckForExistingManufacturer_ReturnsConflict_WhenManufacturerNameExists_AndManufacturerIdIsGreaterThanZero()
    {
        var manufacturer1 = "Manufacturer1";
        var manufacturer2 = "Manufacturer2";
        var context = Shared.GetInMemoryDbContext();
        var controller = new ManufacturerController(context);

        context.Manufacturers.RemoveRange(context.Manufacturers);
        await context.SaveChangesAsync();

        context.Manufacturers.AddRange(new List<ManufacturerModel>
        {
            new() { Name = manufacturer1, StatusId = 1 },
            new() { Name = manufacturer2, StatusId = 2 },
        });
        await context.SaveChangesAsync();

        var result = await controller.CheckForExistingManufacturer(manufacturer1, 2);
        Assert.IsType<ActionResult<HttpStatusCode>>(result);
        Assert.IsType<ConflictResult>(result.Result);
    }
}
