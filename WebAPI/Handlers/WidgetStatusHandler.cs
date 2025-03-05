
namespace WebAPI.Handlers;

public class WidgetStatusHandler(ManufacturerManagerDbContext context) : IWidgetStatusHandler
{
    private readonly ManufacturerManagerDbContext _context = context;

    public async Task<List<WidgetStatusDTO>> GetWidgetStatusesAsync()
    {
        var widgetStatuses = await _context.WidgetStatuses
            .OrderBy(s => s.StatusName)
            .AsNoTracking()
            .ToListAsync();

        var widgetStatusDTOs = new List<WidgetStatusDTO>();

        foreach (var widgetStatus in widgetStatuses)
        {
            widgetStatusDTOs.Add(new WidgetStatusDTO
            {
                StatusId = widgetStatus.StatusId,
                StatusName = widgetStatus.StatusName,
            });
        }

        return widgetStatusDTOs;
    }
}
