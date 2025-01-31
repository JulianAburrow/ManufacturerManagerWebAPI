
namespace ManufacturerManagerUI.Models;

public class WidgetDTO : IValidatableObject
{
    public int WidgetId { get; set; }

    [Required(ErrorMessage = "{0} is required")]
    [StringLength(100, ErrorMessage = "{0} cannot be more that {1} characters")]
    public string Name { get; set; } = default!;

    [Range(1, int.MaxValue, ErrorMessage = "{0} is required")]
    [Display(Name = "Manufacturer")]
    public int ManufacturerId { get; set; }

    public string? ManufacturerName { get; set; }

    public int? ColourId { get; set; }

    public string? ColourName { get; set; }

    public int? ColourJustificationId { get; set; }

    public string? ColourJustificationText { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "{0} is required")]
    [Display(Name = "Status")]
    public int StatusId { get; set; }

    public string? StatusName { get; set; }

    [Required(ErrorMessage = "{0} is required")]
    [Display(Name = "Cost Price")]
    public decimal CostPrice { get; set; }

    [Required(ErrorMessage = "{0} is required")]
    [Display(Name = "Retail Price")]
    public decimal RetailPrice { get; set; }

    [Required(ErrorMessage = "{0} is required")]
    [Display(Name = "Stock Level")]
    public int StockLevel { get; set; }

    public byte[]? WidgetImage { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var result = new List<ValidationResult>();

        if (ColourId == (int) ColoursEnum.Pink && ColourJustificationId is null) // No ColourJustificationId chosen
        {
            result.Add(new ValidationResult(
                "Colour Justification is required when Colour is Pink",
                    ["WidgetDTO.ColourJustificationId"]
            ));
        }

        return result;
    }
}
