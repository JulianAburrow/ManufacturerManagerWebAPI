using Microsoft.Identity.Client;

namespace Tests;

public class ColourJustificationTests
{
    private const string TestJustification = "TestJustification";

    [Fact]
    public async Task GetColourJustifications_GetsAllColourJustifications()
    {
        var justification1 = "Justification1";
        var justification2 = "Justification2";
        var justification3 = "Justification3";
        var justification4 = "Justification4";
        var context = Shared.GetInMemoryDbContext();
        context.ColourJustifications.RemoveRange(context.ColourJustifications);

        context.ColourJustifications.AddRange(new List<ColourJustificationModel>
        {
            new() { Justification = justification1 },
            new() { Justification = justification2 },
            new() { Justification = justification3 },
            new() { Justification = justification4 },
        });
        await context.SaveChangesAsync();

        var controller = new ColourJustificationController(context);

        var result = await controller.GetColourJustifications();

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var actualJustifications = Assert.IsType<List<ColourJustificationDTO>>(okResult.Value);
        Assert.Equal(4, actualJustifications.Count);
        Assert.Equal(justification1, actualJustifications[0].Justification);
        Assert.Equal(justification2, actualJustifications[1].Justification);
        Assert.Equal(justification3, actualJustifications[2].Justification);
        Assert.Equal(justification4, actualJustifications[3].Justification);
    }

    [Fact]
    public async Task GetColourJustification_GetsCorrectColourJustification()
    {
        var context = Shared.GetInMemoryDbContext();
        context.ColourJustifications.RemoveRange(context.ColourJustifications);
        await context.SaveChangesAsync();

        var controller = new ColourJustificationController(context);
        context.ColourJustifications.Add(new ColourJustificationModel { Justification = TestJustification });
        await context.SaveChangesAsync();

        var result = await controller.GetColourJustification(1);
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var actualJustification = Assert.IsType<ColourJustificationDTO>(okResult.Value);
        Assert.Equal(TestJustification, actualJustification.Justification);
        Assert.Equal(1, actualJustification.ColourJustificationId);
    }

    [Fact]
    public async Task GetColourJustification_ReturnsNotFound_WhenColourJustificationDoesNotExist()
    {
        var context = Shared.GetInMemoryDbContext();
        var controller = new ColourJustificationController(context);

        var result = await controller.GetColourJustification(999);

        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task CreateColourJustification_CreatesColourJustification()
    {
        var context = Shared.GetInMemoryDbContext();
        context.ColourJustifications.RemoveRange(context.ColourJustifications);
        await context.SaveChangesAsync();

        var colourJustificationDTO = new ColourJustificationDTO
        {
            Justification = TestJustification
        };

        var controller = new ColourJustificationController(context);

        var result = await controller.CreateColourJustification(colourJustificationDTO);

        var createdResult = Assert.IsType<CreatedResult>(result);
        var createdColourJustification = await context.ColourJustifications
            .FirstOrDefaultAsync(c => c.Justification == TestJustification);
        Assert.NotNull(createdColourJustification);
        Assert.Equal(TestJustification, createdColourJustification.Justification);
    }

    [Fact]
    public async Task CreateColourJustification_ReturnsConflict_WhenColourJustificationExists()
    {
        var context = Shared.GetInMemoryDbContext();
        context.ColourJustifications.Add(new ColourJustificationModel { Justification = TestJustification });
        await context.SaveChangesAsync();

        var controller = new ColourJustificationController(context);
        var newColourJustification = new ColourJustificationDTO { Justification = TestJustification };

        var result = await controller.CreateColourJustification(newColourJustification);

        Assert.IsType<ConflictResult>(result);
    }

    [Fact]
    public async Task UpdateColourJustification_UpdatesColourJustification()
    {
        var newColourJustification = "NewColourJustification";
        var context = Shared.GetInMemoryDbContext();
        var controller = new ColourJustificationController(context);

        context.ColourJustifications.Add(new ColourJustificationModel { Justification = TestJustification });
        await context.SaveChangesAsync();

        var createdColourJustification = await context.ColourJustifications
            .FirstOrDefaultAsync(c => c.Justification == TestJustification);
        Assert.NotNull(createdColourJustification);

        await controller.UpdateColourJustification(createdColourJustification.ColourJustificationId, new ColourJustificationDTO { Justification = newColourJustification });

        var updatedColourJustification = await context.ColourJustifications
            .FirstOrDefaultAsync(c => c.ColourJustificationId == createdColourJustification.ColourJustificationId);
        Assert.NotNull(updatedColourJustification);

        Assert.Equal(newColourJustification, updatedColourJustification.Justification);
    }

    [Fact]
    public async Task UpdateColourJustification_ReturnsNotFound_WhenColourJustificationDoesNotExist()
    {
        var context = Shared.GetInMemoryDbContext();
        var controller = new ColourJustificationController(context);

        var result = await controller.UpdateColourJustification(999, new ColourJustificationDTO { Justification = TestJustification });
        Assert.IsType<NotFoundResult>(result);
    }
}
