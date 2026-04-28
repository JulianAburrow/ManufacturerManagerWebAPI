namespace WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ColourJustificationController(IColourJustificationHandler colourJustificationHandler, IWidgetHandler widgetHandler) : ControllerBase
{
    private readonly IColourJustificationHandler _colourJustificationHandler = colourJustificationHandler;
    private readonly IWidgetHandler _widgetHandler = widgetHandler;

    [HttpGet]
    public async Task<ActionResult<List<ColourJustificationDTO>>> GetColourJustifications()
    {
        var result = await _colourJustificationHandler.GetColourJustificationsAsync();
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ColourJustificationDTO>> GetColourJustification(int id)
    {
        var result = await _colourJustificationHandler.GetColourJustificationAsync(id);
        return result is ColourJustificationDTO dto ? Ok(dto) : NotFound();
    }

    [HttpPost]
    public async Task<ActionResult> CreateColourJustification(ColourJustificationDTO colourJustificationDTO)
    {
        return await _colourJustificationHandler.CreateColourJustificationAsync(colourJustificationDTO);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateColourJustification(int id, ColourJustificationDTO colourJustificationDTO)
    {
        return await _colourJustificationHandler.UpdateColourJustificationAsync(id, colourJustificationDTO);
    }

    [HttpGet("{id}/widgets")]
    public async Task<ActionResult<List<WidgetDTO>>> GetWidgetsForColourJustification(int id)
    {
        return await _widgetHandler.GetWidgetsForColourJustificationAsync(id);
    }
}
