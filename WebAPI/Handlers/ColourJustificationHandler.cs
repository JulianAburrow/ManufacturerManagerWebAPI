
namespace WebAPI.Handlers;

public class ColourJustificationHandler(ManufacturerManagerDbContext context) : IColourJustificationHandler
{
    private readonly ManufacturerManagerDbContext _context = context;

    public async Task<ActionResult> CreateColourJustificationAsync(ColourJustificationDTO colourJustificationDTO)
    {
        var colourJustification = new ColourJustificationModel
        {
            Justification = colourJustificationDTO.Justification,
        };

        try
        {
            _context.ColourJustifications.Add(colourJustification);
            await _context.SaveChangesAsync();
            colourJustificationDTO.ColourJustificationId = colourJustification.ColourJustificationId;
            return new CreatedResult($"/api/colourjustifications/{colourJustification.ColourJustificationId}", colourJustificationDTO);
        }
        catch(Exception ex)
        {
            return new BadRequestObjectResult($"An error occurred while creating the colour justification: {ex.Message}");
        }
    }

    public async Task<ColourJustificationDTO>? GetColourJustificationAsync(int id)
    {
        var colourJustification = await _context.ColourJustifications
            .Include(c => c.Widgets)
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.ColourJustificationId == id);

        if (colourJustification is null)
        {
            return null;
        }

        var colourJustificationDTO = new ColourJustificationDTO
        {
            ColourJustificationId = colourJustification.ColourJustificationId,
            Justification = colourJustification.Justification,
            WidgetCount = colourJustification.Widgets.Count,
        };

        return colourJustificationDTO;
    }

    public async Task<List<ColourJustificationDTO>> GetColourJustificationsAsync()
    {
        var colourJustifications = await _context.ColourJustifications
            .Include(c => c.Widgets)
            .OrderBy(c => c.Justification)
            .AsNoTracking()
            .ToListAsync();

        var colourJustificationDTOs = new List<ColourJustificationDTO>();
        
        foreach (var colourJustification in colourJustifications)
        {
            colourJustificationDTOs.Add(new ColourJustificationDTO
            {
                ColourJustificationId = colourJustification.ColourJustificationId,
                Justification = colourJustification.Justification,
                WidgetCount = colourJustification.Widgets.Count,
            });
        }

        return colourJustificationDTOs;
    }

    public async Task<ActionResult> UpdateColourJustificationAsync(int id, ColourJustificationDTO colourJustificationDTO)
    {
        var colourJustificationToUpdate = await _context.ColourJustifications
            .FirstOrDefaultAsync(c => c.ColourJustificationId == id);

        if (colourJustificationToUpdate is null)
        {
            return new NotFoundObjectResult("No colour justification with this id could be found");
        }

        colourJustificationToUpdate.Justification = colourJustificationDTO.Justification;

        try
        {
            await _context.SaveChangesAsync();
            return new NoContentResult();
        }
        catch (Exception ex)
        {
            return new BadRequestObjectResult($"An error occurred while updating the colour justification: {ex.Message}");
        }
    }
}
