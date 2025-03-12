namespace WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ManufacturerController(IManufacturerHandler manufacturerHandler) : ControllerBase
{
    private readonly IManufacturerHandler _manufacturerHandler = manufacturerHandler;

    [HttpGet("check/{manufacturerName}/{id}")]
    public async Task<ActionResult<HttpStatusCode>> CheckForExistingManufacturers(string manufacturerName, int id)
    {
        var manufacturers = await _manufacturerHandler.CheckForExistingManufacturerAsync(manufacturerName, id);

        return manufacturers.Count == 0 ? Ok() : Conflict();
    }

    [HttpGet]
    public async Task<ActionResult<List<ManufacturerDTO>>> GetManufacturers()
    {
        return await _manufacturerHandler.GetManufacturersAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ManufacturerDTO>> GetManufacturer(int id)
    {
        return await _manufacturerHandler.GetManufacturerAsync(id);        
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
}
