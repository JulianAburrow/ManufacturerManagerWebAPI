namespace ManufacturerManagerUI.Shared.BasePageClasses;

public class ManufacturerBasePageClass
    : BasePageClass
{
    [Parameter]
    public int ManufacturerId { get; set; }

    protected ManufacturerDTO ManufacturerDTO = null!;

    protected List<ManufacturerStatusDTO> ManufacturerStatusDTOs = [];

    protected bool ManufacturerExists;

    protected string ManufacturersEndpoint { get; set; } = "api/Manufacturer";

    protected string ManufacturerStatusesEndpoint { get; set; } = "api/ManufacturerStatus";

    public static ManufacturerDTO SelectManufacturer { get; set; } = new ManufacturerDTO
    {
        ManufacturerId = GlobalValues.SelectValue,
        Name = GlobalValues.Select,
    };

    public static ManufacturerStatusDTO SelectManufacturerStatus { get; set; } = new ManufacturerStatusDTO
    {
        StatusId = GlobalValues.SelectValue,
        StatusName = GlobalValues.Select,
    };

    protected BreadcrumbItem GetManufacturerHomeBreadcrumbItem(bool isDisabled = false)
    {
        return new("Manufacturers", "/manufacturers/index", isDisabled);
    }
}
