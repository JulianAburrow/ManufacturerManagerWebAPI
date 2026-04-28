namespace Tests;

public class ColourHandlerTests
{
    private readonly ColourHandler _handler;
    private readonly ManufacturerManagerDbContext _context;

    private const string Colour1 = "Red";
    private const string Colour2 = "Blue";
    private const string Colour3 = "Yellow";
    private const string Colour4 = "Green";

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

        Assert.Equal("Blue", result[0].Name);
        Assert.Equal("Green", result[1].Name);
        Assert.Equal("Red", result[2].Name);
        Assert.Equal("Yellow", result[3].Name);
    }

    [Fact]
    public async Task GetColour_GetsCorrectColour()
    {
        await RemoveAllColoursFromContext();

        var colour = new ColourModel { Name = Colour1 };
        _context.Colours.Add(colour);

        _context.Widgets.AddRange(
            new WidgetModel { Name = "W1", Colour = colour },
            new WidgetModel { Name = "W2", Colour = colour }
        );

        await _context.SaveChangesAsync();

        var result = await _handler.GetColourAsync(colour.ColourId);

        Assert.NotNull(result);
        Assert.Equal(Colour1, result.Name);
        Assert.Equal(colour.ColourId, result.ColourId);
        Assert.Equal(2, result.WidgetCount);
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
    public async Task UpdateColour_ReturnsNoContent_WhenColourDoesNotAlreadyExist()
    {
        await RemoveAllColoursFromContext();

        _context.Colours.Add(new ColourModel { Name = Colour1 });
        await _context.SaveChangesAsync();

        var createdColour = await _context.Colours
            .FirstOrDefaultAsync(c => c.Name == Colour1);
        Assert.NotNull(createdColour);

        var result = await _handler.UpdateColourAsync(createdColour.ColourId, new ColourDTO { Name = Colour2 });

        Assert.IsType<NoContentResult>(result);

        var updated = await _context.Colours.FindAsync(createdColour.ColourId);
        Assert.NotNull(updated);
        Assert.Equal(Colour2, updated!.Name);
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

        var result = await _handler.UpdateColourAsync(createdColour.ColourId, new ColourDTO { Name = Colour2 });

        Assert.IsType<ConflictObjectResult>(result);
    }

    private async Task RemoveAllColoursFromContext()
    {
        _context.Colours.RemoveRange(_context.Colours);
        await _context.SaveChangesAsync();
    }
}
