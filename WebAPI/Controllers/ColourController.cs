namespace WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ColourController(ManufacturerManagerDbContext context) : ControllerBase
{
    private readonly ManufacturerManagerDbContext _context = context;

    [HttpGet]
    public async Task<ActionResult<List<ColourDTO>>> GetColours()
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

        return Ok(colourDTOs);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ColourDTO>> GetColour(int id)
    {
        var colour = await _context.Colours
            .Include(c => c.Widgets)
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.ColourId == id);
        if (colour is null)
        {
            return NotFound();
        }

        var colourDTO = new ColourDTO
        {
            ColourId = colour.ColourId,
            Name = colour.Name,
            WidgetCount = colour.Widgets.Count,
        };

        return Ok(colourDTO);
    }

    [HttpPost]
    public async Task<ActionResult> CreateColour(ColourDTO colourDTO)
    {
        var colour = new ColourModel
        {
            Name = colourDTO.Name,
        };

        try
        {
            _context.Colours.Add(colour);
            await _context.SaveChangesAsync();
            return Ok();
        }
        catch
        {
            return BadRequest();
        }
    }

    [HttpPut]
    public async Task<ActionResult> UpdateColour(int id, ColourDTO colourDTO)
    {
        var colourToUpdate = _context.Colours
            .FirstOrDefault(c => c.ColourId == id);
        if (colourToUpdate is null)
        {
            return NotFound();
        }

        colourToUpdate.Name = colourDTO.Name;

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
