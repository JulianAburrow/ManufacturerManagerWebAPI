namespace WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class WidgetController(IWidgetHandler widgetHandler) : ControllerBase
{
    private readonly IWidgetHandler _widgetHandler = widgetHandler;

    [HttpGet]
    public async Task<ActionResult<List<WidgetDTO>>> GetWidgets()
    {
        return await _widgetHandler.GetWidgetsAsync();
    }

    [HttpGet("{id}")]
    public async Task<WidgetDTO>? GetWidget(int id)
    {
        return await _widgetHandler.GetWidgetAsync(id);
    }

    [HttpGet("check/{widgetName}/{id}")]
    public async Task<ActionResult<HttpStatusCode>> CheckForExistingWidgetS(string widgetName, int id)
    {
        var widgets = await _widgetHandler.CheckForExistingWidgetAsync(widgetName, id);

        return widgets.Count == 0 ? Ok() : Conflict();
    }

    [HttpGet("widgetsbymanufacturer/{manufacturerId}")]
    public async Task<ActionResult<List<WidgetDTO>>> GetWidgetsForManufacturer(int manufacturerId)
    {
        return await _widgetHandler.GetWidgetsForManufacturerAsync(manufacturerId);
    }

    [HttpGet("widgetsbycolour/{colourId}")]
    public async Task<ActionResult<List<WidgetDTO>>> GetWidgetsForColour(int colourId)
    {
        return await _widgetHandler.GetWidgetsForColourAsync(colourId);
    }

    [HttpGet("widgetsbycolourjustification/{colourJustificationId}")]
    public async Task<ActionResult<List<WidgetDTO>>> GetWidgetsForColourJustificationAsync(int colourJustificationId)
    {
        return await _widgetHandler.GetWidgetsForColourJustificationAsync(colourJustificationId);
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
