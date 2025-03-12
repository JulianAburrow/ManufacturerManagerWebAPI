namespace WebAPI.Interfaces;

public interface IWidgetHandler
{
    Task<List<WidgetDTO>> GetWidgetsAsync();

    Task<WidgetDTO>? GetWidgetAsync(int id);

    Task<ActionResult> CreateWidgetAsync(WidgetDTO widgetDTO);

    Task<ActionResult> UpdateWidgetAsync(int id, WidgetDTO widgetDTO);

    public Task<List<WidgetModel>> CheckForExistingWidgetAsync(string widgetName, int id);

    public Task<List<WidgetDTO>> GetWidgetsForManufacturerAsync(int manufacturerId);

    public Task<List<WidgetDTO>> GetWidgetsForColourAsync(int colourId);

    public Task<List<WidgetDTO>> GetWidgetsForColourJustificationAsync(int colourJustificationId);
}
