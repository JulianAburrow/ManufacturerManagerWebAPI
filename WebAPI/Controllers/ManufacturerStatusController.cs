namespace WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ManufacturerStatusController(ManufacturerManagerDbContext context) : ControllerBase
{
    private readonly ManufacturerManagerDbContext _context = context;

    [HttpGet]
    public async Task<ActionResult<List<ManufacturerStatusDTO>>> GetManufacturerStatuses()
    {
        var manufacturerStatuses = await _context.ManufacturerStatuses
            .OrderBy(s => s.StatusName)
            .AsNoTracking()
            .ToListAsync();

        var manufacturerStatusDTOs = new List<ManufacturerStatusDTO>();

        foreach (var status in manufacturerStatuses)
        {
            manufacturerStatusDTOs.Add(new ManufacturerStatusDTO
            {
                StatusId = status.StatusId,
                StatusName = status.StatusName,
            });
        }

        return Ok(manufacturerStatusDTOs);
    }
}
