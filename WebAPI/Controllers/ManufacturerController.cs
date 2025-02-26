namespace WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ManufacturerController(ManufacturerManagerDbContext context) : ControllerBase
{
    private readonly ManufacturerManagerDbContext _context = context;

    [HttpGet]
    public async Task<ActionResult<List<ManufacturerDTO>>> GetManufacturers()
    {
        var manufacturers = await _context.Manufacturers
                .Include(m => m.Widgets)
                .Include(m => m.Status)
                .OrderBy(m => m.Name)
                .AsNoTracking()
                .ToListAsync();
        var manufacturerDTOs = new List<ManufacturerDTO>();
        foreach (var manufacturer in manufacturers)
        {
            manufacturerDTOs.Add(new ManufacturerDTO
            {
                ManufacturerId = manufacturer.ManufacturerId,
                Name = manufacturer.Name,
                StatusId = manufacturer.StatusId,
                StatusName = manufacturer.Status.StatusName,
                WidgetCount = manufacturer.Widgets.Count,
            });
        }

        return Ok(manufacturerDTOs);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ManufacturerDTO>> GetManufacturer(int id)
    {
        var manufacturer = await _context.Manufacturers
            .Include(m => m.Widgets)
            .Include(m => m.Status)
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.ManufacturerId == id);
        if (manufacturer == null)
        {
            return NotFound();
        }
        var manufacturerDTO = new ManufacturerDTO
        {
            ManufacturerId = manufacturer.ManufacturerId,
            Name = manufacturer.Name,
            StatusId = manufacturer.StatusId,
            StatusName = manufacturer.Status.StatusName,
            WidgetCount = manufacturer.Widgets.Count,
        };

        return Ok(manufacturerDTO);
    }

    [HttpGet("check/{manufacturerName}/{id}")]
    public async Task<ActionResult<HttpStatusCode>> CheckForExistingManufacturer(string manufacturerName, int id)
    {
        var manufacturers = await _context.Manufacturers
            .Where(
                m =>
                    m.Name.Replace(" ", "") == manufacturerName.Replace(" ", ""))
            .ToListAsync();

        if (id > 0)
        {
            manufacturers = manufacturers.Where(m => m.ManufacturerId != id).ToList();
        }

        if (manufacturers.Count > 0)
        {
            return Conflict();
        }

        return Ok();
    }

    [HttpPost]
    public async Task<ActionResult> CreateManufacturer(ManufacturerDTO manufacturerDTO)
    {
        var manufacturer = new ManufacturerModel()
        {
            Name = manufacturerDTO.Name,
            StatusId = manufacturerDTO.StatusId,
        };

        try
        {
            _context.Manufacturers.Add(manufacturer);
            await _context.SaveChangesAsync();
            return Created();
        }
        catch
        {
            return BadRequest();
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateManufacturer(int id, ManufacturerDTO manufacturerDTO)
    {
        var manufacturerToUpdate = _context.Manufacturers
            .FirstOrDefault(m => m.ManufacturerId == id);
        if (manufacturerToUpdate is null)
        {
            return NotFound();
        }
        manufacturerToUpdate.Name = manufacturerDTO.Name;
        manufacturerToUpdate.StatusId = manufacturerDTO.StatusId;

        if (manufacturerToUpdate.StatusId == 2) // Inactive
        {
            var widgets = _context.Widgets
                .Where(w => w.ManufacturerId == manufacturerToUpdate.ManufacturerId);
            foreach (var widget in widgets)
            {
                widget.StatusId = 2;
            }
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
