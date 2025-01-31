namespace WebAPI.DTOs;

public class ColourJustificationDTO
{
    public int ColourJustificationId { get; set; }

    public required string Justification { get; set; }

    public int WidgetCount { get; set; }
}
