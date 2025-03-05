namespace WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ColourJustificationController(IColourJustificationHandler colourJustificationHandler) : ControllerBase
{
    private readonly IColourJustificationHandler _colourJustificationHandler = colourJustificationHandler;

    [HttpGet]
    public async Task<ActionResult<List<ColourJustificationDTO>>> GetColourJustifications()
    {
        return await _colourJustificationHandler.GetColourJustificationsAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ColourJustificationDTO>> GetColourJustification(int id)
    {
        return await _colourJustificationHandler.GetColourJustificationAsync(id);
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

}
