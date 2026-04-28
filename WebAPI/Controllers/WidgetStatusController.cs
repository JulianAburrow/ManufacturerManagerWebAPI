namespace WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class WidgetStatusController(IWidgetStatusHandler widgetStatusHandler) : ControllerBase
{
    private readonly IWidgetStatusHandler _widgetStatusHandler = widgetStatusHandler;

    [HttpGet]
    public async Task<ActionResult<List<WidgetStatusDTO>>> GetWidgetStatuses()
    {
        var statuses = await _widgetStatusHandler.GetWidgetStatusesAsync();
        return Ok(statuses);
    }
}
