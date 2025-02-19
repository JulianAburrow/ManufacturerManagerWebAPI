namespace Tests;

public class ColourTests
{
    private const string TestColour = "TestColour";

    [Fact]
    public async Task GetColours_GetsAllColours()
    {
        var colour1 = "Colour1";
        var colour2 = "Colour2";
        var colour3 = "Colour3";
        var colour4 = "Colour4";
        // Arrange
        var context = Shared.GetInMemoryDbContext();
        context.Colours.RemoveRange(context.Colours);
        await context.SaveChangesAsync();

        context.Colours.AddRange(new List<ColourModel>
        {
            new() { Name = colour1 },
            new() { Name = colour2 },
            new() { Name = colour3 },
            new() { Name = colour4 },
        });
        await context.SaveChangesAsync();

        var controller = new ColourController(context);

        // Act
        var result = await controller.GetColours();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var actualColours = Assert.IsType<List<ColourDTO>>(okResult.Value);
        Assert.Equal(4, actualColours.Count);
        Assert.Equal(colour1, actualColours[0].Name);
        Assert.Equal(colour2, actualColours[1].Name);
        Assert.Equal(colour3, actualColours[2].Name);
        Assert.Equal(colour4, actualColours[3].Name);
    }

    [Fact]
    public async Task GetColour_GetsCorrectColour()
    {
        var colourName = "Colour1";
        var context = Shared.GetInMemoryDbContext();
        context.Colours.RemoveRange(context.Colours);
        await context.SaveChangesAsync();

        var controller = new ColourController(context);
        context.Colours.Add(new ColourModel { Name = colourName });
        await context.SaveChangesAsync();

        var result = await controller.GetColour(1);

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var actualColour = Assert.IsType<ColourDTO>(okResult.Value);
        Assert.Equal(colourName, actualColour.Name);
        Assert.Equal(1, actualColour.ColourId);
    }

    [Fact]
    public async Task GetColour_ReturnsNotFound_WhenColourDoesNotExist()
    {
        var context = Shared.GetInMemoryDbContext();
        var controller = new ColourController(context);

        var result = await controller.GetColour(999);

        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task CreateColour_CreatesColour()
    {
        var context = Shared.GetInMemoryDbContext();

        var colourDTO = new ColourDTO
        {
            Name = TestColour,
        };

        var controller = new ColourController(context);

        var result = await controller.CreateColour(colourDTO);

        var createdResult = Assert.IsType<CreatedResult>(result);
        var createdColour = await context.Colours
            .FirstOrDefaultAsync(c => c.Name == TestColour);
        Assert.NotNull(createdColour);
        Assert.Equal(TestColour, createdColour.Name);
    }

    [Fact]
    public async Task CreateColour_ReturnsConflict_WhenColourExists()
    {
        var context = Shared.GetInMemoryDbContext();
        context.Colours.Add(new ColourModel { Name = TestColour });
        await context.SaveChangesAsync();

        var controller = new ColourController(context);
        var newColour = new ColourDTO { Name = TestColour };

        var result = await controller.CreateColour(newColour);

        Assert.IsType<ConflictResult>(result);
    }

    [Fact]
    public async Task UpdateColour_UpdatesColour()
    {
        var newColour = "NewColour";
        var context = Shared.GetInMemoryDbContext();
        var controller = new ColourController(context);

        context.Colours.Add(new ColourModel { Name = TestColour });
        await context.SaveChangesAsync();

        var createdColour = await context.Colours
            .FirstOrDefaultAsync(c => c.Name == TestColour);
        Assert.NotNull(createdColour);

        await controller.UpdateColour(createdColour.ColourId, new ColourDTO { Name = newColour });

        var updatedColour = await context.Colours
            .FirstOrDefaultAsync(c => c.ColourId == createdColour.ColourId);
        Assert.NotNull(updatedColour);

        Assert.Equal(newColour, updatedColour.Name);
    }

    [Fact]
    public async Task UpdateColour_ReturnsNotFound_WhenColourDoesNotExist()
    {
        var context = Shared.GetInMemoryDbContext();
        var controller = new ColourController(context);

        var result = await controller.UpdateColour(999, new ColourDTO { Name = TestColour });
        Assert.IsType<NotFoundResult>(result);
    }
}
