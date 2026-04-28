namespace WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class WidgetController(IWidgetHandler widgetHandler) : ControllerBase
{
    private readonly IWidgetHandler _widgetHandler = widgetHandler;

    [HttpGet]
    public async Task<ActionResult<List<WidgetDTO>>> GetWidgets()
    {
        var widgets = await _widgetHandler.GetWidgetsAsync();
        return Ok(widgets);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<WidgetDTO>> GetWidget(int id)
    {
        var result = await _widgetHandler.GetWidgetAsync(id);
        return result is WidgetDTO dto ? Ok(dto) : NotFound();
    }

    [HttpPost]
    public async Task<ActionResult> CreateWidget(WidgetDTO widgetDTO)
    {
        return await _widgetHandler.CreateWidgetAsync(widgetDTO);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateWidget(int id, WidgetDTO widgetDTO)
    {
        return await _widgetHandler.UpdateWidgetAsync(id, widgetDTO);
    }
}
