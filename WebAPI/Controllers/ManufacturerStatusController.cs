namespace WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ManufacturerStatusController(IManufacturerStatusHandler manufacturerStatusHandler) : ControllerBase
{
    private readonly IManufacturerStatusHandler _manufacturerStatusHandler = manufacturerStatusHandler;

    [HttpGet]
    public async Task<ActionResult<List<ManufacturerStatusDTO>>> GetManufacturerStatuses()
    {
        return Ok(await _manufacturerStatusHandler.GetManufacturerStatusesAsync());
    }
}
