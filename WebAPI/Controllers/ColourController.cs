namespace WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ColourController(IColourHandler colourHandler, IWidgetHandler widgetHandler) : ControllerBase
{
    private readonly IColourHandler _colourHandler = colourHandler;
    private readonly IWidgetHandler _widgetHandler = widgetHandler;

    [HttpGet]
    public async Task<ActionResult<List<ColourDTO>>> GetColours()
    {
        var colours = await _colourHandler.GetColoursAsync();
        return Ok(colours);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ColourDTO>> GetColour(int id)
    {
        var result = await _colourHandler.GetColourAsync(id);
        return result is ColourDTO dto ? Ok(dto) : NotFound();
    }

    [HttpPost]
    public async Task<ActionResult> CreateColour(ColourDTO colourDTO)
    {
        return await _colourHandler.CreateColourAsync(colourDTO);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateColour(int id, ColourDTO colourDTO)
    {
        return await _colourHandler.UpdateColourAsync(id, colourDTO);
    }

    [HttpGet("{id}/widgets")]
    public async Task<ActionResult<List<WidgetDTO>>> GetWidgetsForColour(int id)
    {
        var widgets = await _widgetHandler.GetWidgetsForColourAsync(id);
        return Ok(widgets);
    }
}
