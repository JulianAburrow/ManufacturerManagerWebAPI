namespace WebAPI.Handlers;

public class ColourHandler(ManufacturerManagerDbContext context) : IColourHandler
{
    private readonly ManufacturerManagerDbContext _context = context;

    public async Task<ActionResult> CreateColourAsync(ColourDTO colourDTO)
    {
        if (_context.Colours.Any(c => c.Name == colourDTO.Name))
        {
            return new ConflictObjectResult("A colour with the same name already exists.");
        }

        var colour = new ColourModel
        {
            Name = colourDTO.Name,
        };

        try
        {
            _context.Colours.Add(colour);
            await _context.SaveChangesAsync();
            colourDTO.ColourId = colour.ColourId;
            return new CreatedResult($"/api/colours/{colour.ColourId}", colourDTO);
        }
        catch (Exception ex)
        {
            return new BadRequestObjectResult($"An error occurred while creating the colour: {ex.Message}");
        }
    }

    public async Task<ColourDTO>? GetColourAsync(int id)
    {
        var colour = await _context.Colours
            .Include(c => c.Widgets)
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.ColourId == id);

        if (colour is null)
        {
            return null;
        }

        var colourDTO = new ColourDTO
        {
            ColourId = colour.ColourId,
            Name = colour.Name,
            WidgetCount = colour.Widgets.Count,
        };

        return colourDTO;
    }

    public async Task<List<ColourDTO>> GetColoursAsync()
    {
        var colours = await _context.Colours
            .Include(c => c.Widgets)
            .OrderBy(c => c.Name)
            .AsNoTracking()
            .ToListAsync();

        var colourDTOs = new List<ColourDTO>();

        foreach (var colour in colours)
        {
            colourDTOs.Add(new ColourDTO
            {
                ColourId = colour.ColourId,
                Name = colour.Name,
                WidgetCount = colour.Widgets.Count,
            });
        }

        return colourDTOs;
    }

    public async Task<ActionResult> UpdateColourAsync(int id, ColourDTO colourDTO)
    {
        var colourToUpdate = await _context.Colours
            .FirstOrDefaultAsync(c => c.ColourId == id);

        if (colourToUpdate is null)
        {
            return new NotFoundObjectResult("No Colour with this id could be found");
        }

        colourToUpdate.Name = colourDTO.Name;

        if (_context.Colours.Any(c => c.Name == colourToUpdate.Name && c.ColourId != id))
        {
            return new ConflictObjectResult("A colour with this name already exists");
        }

        try
        {
            await _context.SaveChangesAsync();
            return new NoContentResult();
        }
        catch (Exception ex)
        {
            return new BadRequestObjectResult($"An error occurred while updating the Colour: {ex.Message}");
        }
    }
}
