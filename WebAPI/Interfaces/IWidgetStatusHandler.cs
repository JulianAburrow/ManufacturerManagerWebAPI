namespace WebAPI.Interfaces;

public interface IWidgetStatusHandler
{
    Task<List<WidgetStatusDTO>> GetWidgetStatusesAsync();
}
