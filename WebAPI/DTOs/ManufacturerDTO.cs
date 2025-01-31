namespace WebAPI.DTOs;

public class ManufacturerDTO
{
    public int ManufacturerId { get; set; }

    public string Name { get; set; } = default!;

    public int StatusId { get; set; }

    public string? StatusName { get; set; } = null!;

    public int WidgetCount { get; set; }
}
