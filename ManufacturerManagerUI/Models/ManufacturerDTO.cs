

namespace ManufacturerManagerUI.Models;

public class ManufacturerDTO
{
    public int ManufacturerId { get; set; }

    [Required(ErrorMessage = "{0} is required")]
    [StringLength(100, ErrorMessage = "{0} cannot be more that {1} characters")]
    public string Name { get; set; } = default!;

    [Range(1, int.MaxValue, ErrorMessage = "{0} is required")]
    [Display(Name = "Status")]
    public int StatusId { get; set; }

    public string? StatusName { get; set; } = null!;

    public int WidgetCount { get; set; }
}
