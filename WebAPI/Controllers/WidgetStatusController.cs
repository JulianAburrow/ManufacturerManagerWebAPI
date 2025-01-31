namespace WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class WidgetStatusController(ManufacturerManagerDbContext context) : ControllerBase
{
    private readonly ManufacturerManagerDbContext _context = context;

    [HttpGet]
    public async Task<ActionResult<List<WidgetStatusDTO>>> GetWidgetStatuses()
    {
        var widgetStatuses = await _context.WidgetStatuses
     .OrderBy(s => s.StatusName)
     .AsNoTracking()
     .ToListAsync();
        var widgetStatusDTOs = new List<WidgetStatusDTO>();
        foreach (var status in widgetStatuses)
        {
            widgetStatusDTOs.Add(new WidgetStatusDTO
            {
                StatusId = status.StatusId,
                StatusName = status.StatusName,
            });
        }
        return Ok(widgetStatusDTOs);
    }
}
