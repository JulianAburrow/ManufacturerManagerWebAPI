using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Tests;

public class ManufacturerHandlerTests
{
    private readonly ManufacturerHandler _handler;
    private readonly ManufacturerManagerDbContext _context;

    private const string ManufacturerName1 = "ManufacturerName1";
    private const string ManufacturerName2 = "ManufacturerName2";

    private const string ActiveStatus = "Active";
    private const string InactiveStatus = "Inactive";

    private ManufacturerModel ManufacturerModel1 = new()
    {
        Name = ManufacturerName1,
        Status = new ManufacturerStatusModel { StatusId = 1, StatusName = ActiveStatus },
        StatusId = 1,
    };

    private ManufacturerModel ManufacturerModel2 = new()
    {
        Name = ManufacturerName2,
        Status = new ManufacturerStatusModel { StatusId = 2, StatusName = InactiveStatus },
        StatusId = 2,
    };

    private ManufacturerDTO ManufacturerDTO1 = new()
    {
        Name = ManufacturerName1,
        StatusId = 1,
    };

    private ManufacturerDTO ManufacturerDTO2 = new()
    {
        Name = ManufacturerName2,
        StatusId = 2,
    };

    public ManufacturerHandlerTests()
    {
        _context = Shared.GetInMemoryDbContext();
        _handler = new ManufacturerHandler(_context);
    }

    [Fact]
    public async Task GetManufacturers_GetsAllManufacturers()
    {
        await RemoveAllManufacturersFromContext();

        _context.Manufacturers.AddRange(new List<ManufacturerModel>
        {
            ManufacturerModel1,
            ManufacturerModel2,
        });
        await _context.SaveChangesAsync();

        var result = await _handler.GetManufacturersAsync();

        Assert.Equal(2, result.Count);
        Assert.Equal(ManufacturerName1, result[0].Name);
        Assert.Equal(1, result[0].StatusId);
        Assert.Equal(ManufacturerName2, result[1].Name);
        Assert.Equal(2, result[1].StatusId);
    }

    [Fact]
    public async Task GetManufacturer_GetsCorrectManufacturer()
    {
        await RemoveAllManufacturersFromContext();

        _context.Manufacturers.Add(ManufacturerModel2);
        await _context.SaveChangesAsync();

        var result = await _handler.GetManufacturerAsync(1);

        Assert.NotNull(result);
        Assert.Equal(ManufacturerName2, result.Name);
        Assert.Equal(2, result.StatusId);
    }

    [Fact]
    public async Task GetManufacturer_ReturnsNull_WhenManufacturerDoesNotExist()
    {
        var result = await _handler.GetManufacturerAsync(999);

        Assert.Null(result);
    }

    [Fact]
    public async Task CreateManufacturer_ReturnsCreatedResult_WhenManufacturerDoesNotAlreadyExist()
    {
        await RemoveAllManufacturersFromContext();

        var manufacturerDTO = new ManufacturerDTO
        {
            Name = ManufacturerName1,
            StatusId = 1,
        };

        var result = await _handler.CreateManufacturerAsync(manufacturerDTO);

        var createdResult = Assert.IsType<CreatedResult>(result);
        var createdManufacturer = await _context.Manufacturers
            .FirstOrDefaultAsync(m => m.Name == ManufacturerName1);
        Assert.NotNull(createdManufacturer);
        Assert.Equal(ManufacturerName1, createdManufacturer.Name);
        Assert.Equal(1, createdManufacturer.StatusId);
    }

    [Fact]
    public async Task CreateManufacturer_ReturnsConflict_WhenManufacturerAlreadyExists()
    {
        await RemoveAllManufacturersFromContext();

        _context.Manufacturers.Add(ManufacturerModel1);
        await _context.SaveChangesAsync();

        var result = await _handler.CreateManufacturerAsync(ManufacturerDTO1);

        Assert.IsType<ConflictObjectResult>(result);
    }

    [Fact]
    public async Task UpdateManufacturer_ReturnsOk_WhenManufacturerDoesNotAlreadyExist()
    {
        await RemoveAllManufacturersFromContext();

        _context.Manufacturers.Add(ManufacturerModel1);
        await _context.SaveChangesAsync();

        var createdManufacturer = await _context.Manufacturers
            .FirstOrDefaultAsync(m => m.Name == ManufacturerName1);
        Assert.NotNull(createdManufacturer);

        var result = await _handler.UpdateManufacturerAsync(1, ManufacturerDTO2);

        Assert.IsType<OkResult>(result);
    }

    [Fact]
    public async Task UpdateManufacturer_ReturnsNotFound_WhenManufacturerDoesNotExist()
    {
        var result = await _handler.UpdateManufacturerAsync(999, ManufacturerDTO2);

        Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public async Task UpdateManufacturer_ReturnsConflict_WhenManufacturerExists()
    {
        await RemoveAllManufacturersFromContext();

        _context.Manufacturers.AddRange(new List<ManufacturerModel>
        {
            ManufacturerModel1,
            ManufacturerModel2,
        });
        await _context.SaveChangesAsync();

        var createdManufacturer = await _context.Manufacturers
            .FirstOrDefaultAsync(m => m.Name == ManufacturerName1);
        Assert.NotNull(createdManufacturer);

        createdManufacturer.Name = ManufacturerName2;
        var result = await _handler.UpdateManufacturerAsync(
            createdManufacturer.ManufacturerId,
            new ManufacturerDTO { Name = ManufacturerName2, StatusId = 2 });

        Assert.IsType<ConflictObjectResult>(result);

    }

    private async Task RemoveAllManufacturersFromContext()
    {
        _context.RemoveRange(_context.Manufacturers);
        await _context.SaveChangesAsync();
    }
}
