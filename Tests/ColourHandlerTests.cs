namespace Tests;

public class ColourHandlerTests
{
    private readonly ColourHandler _handler;
    private readonly ManufacturerManagerDbContext _context;

    private const string Colour1 = "Colour1";
    private const string Colour2 = "Colour2";
    private const string Colour3 = "Colour3";
    private const string Colour4 = "Colour4";

    public ColourHandlerTests()
    {
        _context = Shared.GetInMemoryDbContext();
        _handler = new ColourHandler(_context);
    }

    [Fact]
    public async Task GetColours_GetsAllColours()
    {
        await RemoveAllColoursFromContext();

        _context.Colours.AddRange(new List<ColourModel>
        {
            new() { Name = Colour1 },
            new() { Name = Colour2 },
            new() { Name = Colour3 },
            new() { Name = Colour4 },
        });
        await _context.SaveChangesAsync();

        var result = await _handler.GetColoursAsync();

        Assert.Equal(4, result.Count);
        Assert.Equal(Colour1, result[0].Name);
        Assert.Equal(Colour2, result[1].Name);
        Assert.Equal(Colour3, result[2].Name);
        Assert.Equal(Colour4, result[3].Name);
    }

    [Fact]
    public async Task GetColour_GetsCorrectColour()
    {
        await RemoveAllColoursFromContext();

        _context.Colours.Add(new ColourModel { Name = Colour1 });
        await _context.SaveChangesAsync();

        var result = await _handler.GetColourAsync(1);

        Assert.NotNull(result);
        Assert.Equal(Colour1, result.Name);
        Assert.Equal(1, result.ColourId);
    }

    [Fact]
    public async Task GetColour_ReturnsNull_WhenColourDoesNotExist()
    {
        var result = await _handler.GetColourAsync(999);

        Assert.Null(result);
    }

    [Fact]
    public async Task CreateColour_ReturnsCreatedResult_WhenColourDoesNotAlreadyExist()
    {
        await RemoveAllColoursFromContext();

        var colourDTO = new ColourDTO
        {
            Name = Colour1,
        };

        var result = await _handler.CreateColourAsync(colourDTO);

        var createdResult = Assert.IsType<CreatedResult>(result);
        var createdColour = await _context.Colours
            .FirstOrDefaultAsync(c => c.Name == Colour1);
        Assert.NotNull(createdColour);
        Assert.Equal(Colour1, createdColour.Name);
    }

    [Fact]
    public async Task CreateColour_ReturnsConflict_WhenColourAlreadyExists()
    {
        await RemoveAllColoursFromContext();

        _context.Colours.Add(new ColourModel { Name = Colour1 });
        await _context.SaveChangesAsync();

        var newColour = new ColourDTO { Name = Colour1 };

        var result = await _handler.CreateColourAsync(newColour);

        Assert.IsType<ConflictObjectResult>(result);
    }

    [Fact]
    public async Task UpdateColour_ReturnsOk_WhenColourDoesNotAlreadyExist()
    {
       await RemoveAllColoursFromContext();

        _context.Colours.Add(new ColourModel { Name = Colour1 });            
        await _context.SaveChangesAsync();

        var createdColour = await _context.Colours
            .FirstOrDefaultAsync(c => c.Name == Colour1);
        Assert.NotNull(createdColour);

        var result = await _handler.UpdateColourAsync(createdColour.ColourId, new ColourDTO { Name = Colour2 });

        Assert.IsType<OkResult>(result);
    }

    [Fact]
    public async Task UpdateColour_ReturnsNotFound_WhenColourDoesNotExist()
    {
        var result = await _handler.UpdateColourAsync(999, new ColourDTO { Name = Colour1 });
        Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public async Task UpdateColour_ReturnsConflict_WhenColourExists()
    {
        await RemoveAllColoursFromContext();

        _context.Colours.AddRange(new List<ColourModel>
        {
            new() { Name = Colour1 },
            new() { Name = Colour2 },
        });

        await _context.SaveChangesAsync();

        var createdColour = await _context.Colours
            .FirstOrDefaultAsync(c => c.Name == Colour1);
        Assert.NotNull(createdColour);

        createdColour.Name = Colour2;
        var result = await _handler.UpdateColourAsync(createdColour.ColourId, new ColourDTO { Name = Colour2 });

        Assert.IsType<ConflictObjectResult>(result);
    }

    private async Task RemoveAllColoursFromContext()
    {
        _context.Colours.RemoveRange(_context.Colours);
        await _context.SaveChangesAsync();
    }
}
