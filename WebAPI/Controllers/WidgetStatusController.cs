namespace WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class WidgetStatusController(IWidgetStatusHandler widgetStatusHandler) : ControllerBase
{
    private readonly IWidgetStatusHandler _widgetStatusHandler = widgetStatusHandler;

    [HttpGet]
    public async Task<List<WidgetStatusDTO>> GetWidgetStatuses()
    {
        return await _widgetStatusHandler.GetWidgetStatusesAsync();
    }
}
