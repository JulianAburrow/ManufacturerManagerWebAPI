namespace WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ColourJustificationController(ManufacturerManagerDbContext context) : ControllerBase
{
    private readonly ManufacturerManagerDbContext _context = context;

    [HttpGet]
    public async Task<ActionResult<List<ColourJustificationDTO>>> GetColourJustifications()
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

        return Ok(colourJustificationDTOs);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ColourJustificationDTO>> GetColourJustification(int id)
    {
        var colourJustification = await _context.ColourJustifications
            .Include(c => c.Widgets)
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.ColourJustificationId == id);
        if (colourJustification is null)
        {
            return NotFound();
        }

        var colourJustificationDTO = new ColourJustificationDTO
        {
            ColourJustificationId = colourJustification.ColourJustificationId,
            Justification = colourJustification.Justification,
            WidgetCount = colourJustification.Widgets.Count,
        };

        return Ok(colourJustificationDTO);
    }

    [HttpPost]
    public async Task<ActionResult> CreateColourJustification(ColourJustificationDTO colourJustificationDTO)
    {
        var colourJustification = new ColourJustificationModel
        {
            Justification = colourJustificationDTO.Justification,
        };

        if (_context.ColourJustifications.Any(
                c =>
                    c.Justification.Replace(" ", "") == colourJustificationDTO.Justification.Replace(" ", "")
                )
           )
        {
            return Conflict();
        }

        try
        {
            _context.ColourJustifications.Add(colourJustification);
            await _context.SaveChangesAsync();
            return Ok();
        }
        catch
        {
            return BadRequest();
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateColourJustification(int id, ColourJustificationDTO colourJustificationDTO)
    {
        var colourJustificationToUpdate = _context.ColourJustifications
            .FirstOrDefault(c => c.ColourJustificationId == id);
        if (colourJustificationToUpdate is null)
        {
            return NotFound();
        }

        colourJustificationToUpdate.Justification = colourJustificationDTO.Justification;

        if (_context.ColourJustifications.Any(
                c =>
                    c.Justification.Replace(" ", "") == colourJustificationDTO.Justification.Replace(" ", "") &&
                    c.ColourJustificationId != id
                )
           )
        {
            return Conflict();
        }

        try
        {
            await _context.SaveChangesAsync();
            return Ok();
        }
        catch
        {
            return BadRequest();
        }
    }

}
