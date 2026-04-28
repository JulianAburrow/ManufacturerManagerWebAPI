namespace WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ManufacturerController(IManufacturerHandler manufacturerHandler, IWidgetHandler widgetHandler) : ControllerBase
{
    private readonly IManufacturerHandler _manufacturerHandler = manufacturerHandler;
    private readonly IWidgetHandler _widgetHandler = widgetHandler;

    [HttpGet]
    public async Task<ActionResult<List<ManufacturerDTO>>> GetManufacturers()
    {
        var manufacturers = await _manufacturerHandler.GetManufacturersAsync();
        return Ok(manufacturers);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ManufacturerDTO>> GetManufacturer(int id)
    {
        var result = await _manufacturerHandler.GetManufacturerAsync(id);
        return result is ManufacturerDTO dto ? Ok(dto) : NotFound();
    }

    [HttpPost]
    public async Task<ActionResult> CreateManufacturer(ManufacturerDTO manufacturerDTO)
    {
        return await _manufacturerHandler.CreateManufacturerAsync(manufacturerDTO);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateManufacturer(int id, ManufacturerDTO manufacturerDTO)
    {
        return await _manufacturerHandler.UpdateManufacturerAsync(id, manufacturerDTO);
    }

    [HttpGet("{id}/widgets")]
    public async Task<ActionResult<List<WidgetDTO>>> GetWidgetsForManufacturer(int id)
    {
        var widgets = await _widgetHandler.GetWidgetsForManufacturerAsync(id);
        return Ok(widgets);
    }
}
