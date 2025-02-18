using Microsoft.AspNetCore.Mvc;
using Moq;
using WebAPI.Controllers;
using WebAPI.DTOs;
using WebAPI.Models;
using Xunit;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Data;
using Microsoft.EntityFrameworkCore;

namespace Tests;

public class ColourTests
{
    private const string TestColour = "TestColour";

    [Fact]
    public async Task GetColours_ReturnsAllColours()
    {
        // Arrange
        var context = Shared.GetInMemoryDbContext();
        context.Colours.AddRange(new List<ColourModel>
        {
            new() { Name = "Red" },
            new() { Name = "Blue" },
            new() { Name = "Green" },
            new() { Name = "Yellow" },
        });
        await context.SaveChangesAsync();

        var controller = new ColourController(context);

        // Act
        var result = await controller.GetColours();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var actualColours = Assert.IsType<List<ColourDTO>>(okResult.Value);
        Assert.Equal(4, actualColours.Count);
        Assert.Equal("Blue", actualColours[0].Name);
        Assert.Equal("Green", actualColours[1].Name);
        Assert.Equal("Red", actualColours[2].Name);
        Assert.Equal("Yellow", actualColours[3].Name);
    }

    [Fact]
    public async Task GetColour_ReturnsCorrectColour()
    {
        var context = Shared.GetInMemoryDbContext();
        context.Colours.RemoveRange(context.Colours);
        await context.SaveChangesAsync();

        var controller = new ColourController(context);
        context.Colours.Add(new ColourModel { Name = "Red" });
        await context.SaveChangesAsync();

        var result = await controller.GetColour(1);

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var actualColour = Assert.IsType<ColourDTO>(okResult.Value);
        Assert.Equal("Red", actualColour.Name);
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
        var context = Shared.GetInMemoryDbContext();
        var controller = new ColourController(context);

        context.Colours.Add(new ColourModel { Name = TestColour });
        await context.SaveChangesAsync();

        var createdColour = await context.Colours
            .FirstOrDefaultAsync(c => c.Name == TestColour);
        Assert.NotNull(createdColour);

        createdColour.Name = "NewColour";
        await controller.UpdateColour(createdColour.ColourId, new ColourDTO { Name = createdColour.Name });

        var updatedColour = await context.Colours
            .FirstOrDefaultAsync(c => c.ColourId == createdColour.ColourId);
        Assert.NotNull(updatedColour);

        Assert.Equal("NewColour", updatedColour.Name);
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
