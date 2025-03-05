using System.Drawing;

namespace Tests;

public class ColourJustificationHandlerTests
{
    private readonly ColourJustificationHandler _handler;
    private readonly ManufacturerManagerDbContext _context;

    private const string Justification1 = "Justification1";
    private const string Justification2 = "Justification2";
    private const string Justification3 = "Justification3";
    private const string Justification4 = "Justification4";

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
        Assert.Equal(Justification1, result[0].Justification);
        Assert.Equal(Justification2, result[1].Justification);
        Assert.Equal(Justification3, result[2].Justification);
        Assert.Equal(Justification4, result[3].Justification);
    }

    [Fact]
    public async Task GetColourJustification_GetsCorrectColourJustification()
    {
        await RemoveAllColourJustificationsFromContext();

        _context.ColourJustifications.Add(new ColourJustificationModel { Justification = Justification1 });
        await _context.SaveChangesAsync();

        var result = await _handler.GetColourJustificationAsync(1);

        Assert.NotNull(result);
        Assert.Equal(Justification1, result.Justification);
        Assert.Equal(1, result.ColourJustificationId);
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

        Assert.IsType<OkResult>(result);
    }

    [Fact]
    public async Task UpdateColourJustification_ReturnsNotFound_WhenColourJustificationDoesNotAlreadyExist()
    {
        var result = await _handler.UpdateColourJustificationAsync(999, new ColourJustificationDTO { Justification = Justification1 });
        Assert.IsType<NotFoundObjectResult>(result);
    }

    private async Task RemoveAllColourJustificationsFromContext()
    {
        _context.ColourJustifications.RemoveRange(_context.ColourJustifications);
        await _context.SaveChangesAsync();
    }
}
