namespace ManufacturerManagerUI.Models;

public class ColourJustificationDTO
{
    public int ColourJustificationId { get; set; }

    [Required(ErrorMessage = "{0} is required")]
    [StringLength(100, ErrorMessage = "{0} cannot be more that {1} characters")]
    public string Justification { get; set; } = default!;

    public int WidgetCount { get; set; }
}
