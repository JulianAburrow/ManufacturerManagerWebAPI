namespace ManufacturerManagerUI.Shared.BasePageClasses;

public class WidgetBasePageClass(HttpClient http, NavigationManager navigationManager)
    : BasePageClass(http, navigationManager)
{
    [Parameter]
    public int WidgetId { get; set; }

    protected WidgetDTO WidgetDTO = new();
    protected List<WidgetStatusDTO> WidgetStatusDTOs = [];
    protected List<ManufacturerDTO> ManufacturerDTOs = [];
    protected List<ColourDTO> ColourDTOs = [];
    protected List<ColourJustificationDTO> ColourJustificationDTOs = [];
    private static string Select { get; set; } = "Select";
    private static string None { get; set; } = "None";
    protected string WidgetStatusesEndpoint { get; set; } = "api/WidgetStatus";
    public static ManufacturerDTO SelectManufacturer = new ManufacturerDTO
    {
        ManufacturerId = 0,
        Name = Select,
    };
    public static ColourDTO NoneColour { get; set; } = new ColourDTO
    {
        ColourId = 0,
        Name = None,
    };
    public static ColourJustificationDTO NoneColourJustification { get; set; } = new ColourJustificationDTO
    {
        ColourJustificationId = 0,
        Justification = None,
    };
    public static WidgetStatusDTO SelectWidgetStatus { get; set; } = new WidgetStatusDTO
    {
        StatusId = 0,
        StatusName = Select,
    };
}