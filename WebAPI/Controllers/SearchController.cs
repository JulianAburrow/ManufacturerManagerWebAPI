namespace WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SearchController(ManufacturerManagerDbContext context) : ControllerBase
{
    private readonly ManufacturerManagerDbContext _context = context;

    [HttpGet("searchmanufacturers")]
    public async Task<ActionResult<List<ManufacturerDTO>>> SearchManufacturers(ManufacturerDTO manufacturerDTO)
    {
        var manufacturerResults = await _context.Manufacturers
            .Include(m => m.Widgets)
            .Include(m => m.Status)
            .OrderBy(m => m.Name)
            .Where(m => m.Name.Contains(manufacturerDTO.Name))
            .AsNoTracking()
            .ToListAsync();
        if (manufacturerDTO.StatusId > 0)
        {
            manufacturerResults =
                [.. manufacturerResults.Where
                    (m =>
                        m.StatusId == manufacturerDTO.StatusId)];
        }
        var manufacturerDTOs = new List<ManufacturerDTO>();
        foreach (var manufacturerResult in manufacturerResults)
        {
            manufacturerDTOs.Add(new ManufacturerDTO
            {
                ManufacturerId = manufacturerResult.ManufacturerId,
                Name = manufacturerResult.Name,
                StatusId = manufacturerResult.StatusId,
                StatusName = manufacturerResult.Status.StatusName,
                WidgetCount = manufacturerResult.Widgets.Count,
            });
        }

        return Ok(manufacturerDTOs);
    }

    [HttpGet]
    public async Task<ActionResult<List<WidgetDTO>>> SearchWidgets(WidgetDTO widgetDTO)
    {
        var widgetDTOs = new List<WidgetDTO>();




        return Ok(widgetDTOs);
    }
}
