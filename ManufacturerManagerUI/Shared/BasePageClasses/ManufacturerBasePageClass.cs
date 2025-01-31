namespace ManufacturerManagerUI.Shared.BasePageClasses;

public class ManufacturerBasePageClass(HttpClient http, NavigationManager navigationManager)
    : BasePageClass(http, navigationManager)
{
    [Parameter]
    public int ManufacturerId { get; set; }

    protected ManufacturerDTO ManufacturerDTO = new();
    protected List<ManufacturerStatusDTO> ManufacturerStatusDTOs = [];

    protected string ManufacturersEndpoint { get; set; } = "api/Manufacturer";
    protected string ManufacturerStatusesEndpoint { get; set; } = "api/ManufacturerStatus";

    private static string Select { get; set; } = "Select";

    public static ManufacturerDTO SelectManufacturer { get; set; } = new ManufacturerDTO
    {
        ManufacturerId = 0,
        Name = Select,
    };

    public static ManufacturerStatusDTO SelectManufacturerStatus { get; set; } = new ManufacturerStatusDTO
    {
        StatusId = 0,
        StatusName = Select,
    };
}
