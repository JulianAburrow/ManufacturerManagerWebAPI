using System.Net.WebSockets;

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
        ManufacturerId = SelectValue,
        Name = Select,
    };

    public static ManufacturerStatusDTO SelectManufacturerStatus { get; set; } = new ManufacturerStatusDTO
    {
        StatusId = SelectValue,
        StatusName = Select,
    };

    protected static BreadcrumbItem GetManufacturerHomeBreadcrumbItem(bool isDisabled = false)
    {
        return new("Manufacturers", "/manufacturers/index", isDisabled);
    }
}
