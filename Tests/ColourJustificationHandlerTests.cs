using System.Drawing;

namespace Tests;

public class ColourJustificationHandlerTests
{
    private readonly ColourJustificationHandler _handler;
    private readonly ManufacturerManagerDbContext _context;

    private const string Justification1 = "A reason";
    private const string Justification2 = "C reason";
    private const string Justification3 = "D reason";
    private const string Justification4 = "B reason";

    public ColourJustificationHandlerTests()
    {
        _context = Shared.GetInMemoryDbContext();
        _handler = new ColourJustificationHandler(_context);
    }

    [Fact]
    public async Task GetColourJustifications_GetsAllColourJustifications()
    {
        await RemoveAllColourJustificationsFromContext();

        _context.ColourJustifications.AddRange(new List<ColourJustificationModel>
        {
            new() { Justification = Justification1 },
            new() { Justification = Justification2 },
            new() { Justification = Justification3 },
            new() { Justification = Justification4 },
        });
        await _context.SaveChangesAsync();

        var result = await _handler.GetColourJustificationsAsync();

        Assert.Equal(4, result.Count);
        Assert.Equal("A reason", result[0].Justification);
        Assert.Equal("B reason", result[1].Justification);
        Assert.Equal("C reason", result[2].Justification);
        Assert.Equal("D reason", result[3].Justification);
    }

    [Fact]
    public async Task GetColourJustification_GetsCorrectColourJustification()
    {
        await RemoveAllColourJustificationsFromContext();

        var widgets = new List<WidgetModel>
        {
            new() { Name = "Widget1" },
            new() { Name = "Widget2" },
        };
        _context.ColourJustifications.Add(new ColourJustificationModel { Justification = Justification1, Widgets = widgets });
        await _context.SaveChangesAsync();

        var result = await _handler.GetColourJustificationAsync(1);

        Assert.NotNull(result);
        Assert.Equal(Justification1, result.Justification);
        Assert.Equal(1, result.ColourJustificationId);
        Assert.Equal(2, result.WidgetCount);
    }

    [Fact]
    public async Task GetColourJustification_ReturnsNotFound_WhenColourJustificationDoesNotExist()
    {
        var result = await _handler.GetColourJustificationAsync(999);

        Assert.Null(result);
    }

    [Fact]
    public async Task CreateColourJustification_ReturnsCreatedResult_WhenColourJustificationDoesNotAlreadyExist()
    {
        await RemoveAllColourJustificationsFromContext();

        var colourJustificationDTO = new ColourJustificationDTO
        {
            Justification = Justification1,
        };

        var result = await _handler.CreateColourJustificationAsync(colourJustificationDTO);

        var createdResult = Assert.IsType<CreatedResult>(result);
        var createdColourJustification = await _context.ColourJustifications
            .FirstOrDefaultAsync(c => c.Justification == Justification1);
        Assert.NotNull(createdColourJustification);
        Assert.Equal(Justification1, createdColourJustification.Justification);
        Assert.Equal(1, createdColourJustification.ColourJustificationId);
    }

    [Fact]
    public async Task UpdateColourJustification_ReturnsOk_WhenColourSuccessfullyUpdated()
    {
        await RemoveAllColourJustificationsFromContext();

        _context.ColourJustifications.Add(new ColourJustificationModel { Justification = Justification1 });
        await _context.SaveChangesAsync();

        var createdColourJustification = await _context.ColourJustifications
            .FirstOrDefaultAsync(c => c.Justification == Justification1);
        Assert.NotNull(createdColourJustification);

        var result = await _handler.UpdateColourJustificationAsync(createdColourJustification.ColourJustificationId, new ColourJustificationDTO { Justification = Justification2 });

        Assert.IsType<NoContentResult>(result);

        var updated = await _context.ColourJustifications.FindAsync(createdColourJustification.ColourJustificationId);
        Assert.NotNull(updated);
        Assert.Equal(Justification2, updated!.Justification);
    }

    [Fact]
    public async Task UpdateColourJustification_ReturnsNotFound_WhenColourJustificationDoesNotAlreadyExist()
    {
        var result = await _handler.UpdateColourJustificationAsync(999, new ColourJustificationDTO { Justification = Justification1 });
        Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public async Task UpdateColourJustification_ReturnsConflict_WhenColourJustificationExists()
    {
        await RemoveAllColourJustificationsFromContext();

        _context.ColourJustifications.AddRange(new List<ColourJustificationModel>
        {
            new() { Justification = Justification1 },
            new() { Justification = Justification2 },
        });

        await _context.SaveChangesAsync();

        var createdColourJustification = await _context.ColourJustifications
            .FirstOrDefaultAsync(c => c.Justification == Justification1);
        Assert.NotNull(createdColourJustification);

        var result = await _handler.UpdateColourJustificationAsync(createdColourJustification.ColourJustificationId, new ColourJustificationDTO { Justification = Justification2 });

        Assert.IsType<ConflictObjectResult>(result);
    }

    private async Task RemoveAllColourJustificationsFromContext()
    {
        _context.ColourJustifications.RemoveRange(_context.ColourJustifications);
        await _context.SaveChangesAsync();
    }
}
