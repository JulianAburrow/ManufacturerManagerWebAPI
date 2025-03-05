namespace WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ColourController(IColourHandler colourHandler) : ControllerBase
{
    private readonly IColourHandler _colourHandler = colourHandler;

    [HttpGet]
    public async Task<ActionResult<List<ColourDTO>>> GetColours()
    {
        return await _colourHandler.GetColoursAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ColourDTO>> GetColour(int id)
    {
        return await _colourHandler.GetColourAsync(id);
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
}
