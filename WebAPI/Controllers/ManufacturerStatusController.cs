namespace WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ManufacturerStatusController(IManufacturerStatusHandler manufacturerStatusHandler) : ControllerBase
{
    private readonly IManufacturerStatusHandler _manufacturerStatusHandler = manufacturerStatusHandler;

    [HttpGet]
    public async Task<ActionResult<List<ManufacturerStatusDTO>>> GetManufacturerStatuses()
    {
        var statuses = await _manufacturerStatusHandler.GetManufacturerStatusesAsync();
        return Ok(statuses);
    }
}
