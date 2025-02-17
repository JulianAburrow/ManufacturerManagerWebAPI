using System.Threading.Tasks;

namespace ManufacturerManagerUI.Shared.BasePageClasses;

public class WidgetBasePageClass
    : BasePageClass
{
    [Parameter]
    public int WidgetId { get; set; }

    protected WidgetDTO WidgetDTO = null!;

    protected List<WidgetStatusDTO> WidgetStatusDTOs = [];

    protected List<ManufacturerDTO> ManufacturerDTOs = [];

    protected List<ColourDTO> ColourDTOs = [];

    protected List<ColourJustificationDTO> ColourJustificationDTOs = [];

    protected bool WidgetExists;

    protected string WidgetStatusesEndpoint { get; set; } = "api/WidgetStatus";

    public static ManufacturerDTO SelectManufacturer = new ManufacturerDTO
    {
        ManufacturerId = GlobalValues.SelectValue,
        Name = GlobalValues.Select,
    };

    public static ColourDTO NoneColour { get; set; } = new ColourDTO
    {
        ColourId = GlobalValues.NoneValue,
        Name = GlobalValues.None,
    };

    public static ColourJustificationDTO NoneColourJustification { get; set; } = new ColourJustificationDTO
    {
        ColourJustificationId = GlobalValues.NoneValue,
        Justification = GlobalValues.None,
    };
    public static WidgetStatusDTO SelectWidgetStatus { get; set; } = new WidgetStatusDTO
    {
        StatusId = GlobalValues.SelectValue,
        StatusName = GlobalValues.Select,
    };
    protected BreadcrumbItem GetWidgetHomeBreadcrumbItem(bool isDisabled = false)
    {
        return new("Widgets", "/widgets/index", isDisabled);
    }

    protected async Task CheckForExistingWidget()
    {
        var checkResponse = await Http
            .GetAsync($"{WidgetsEndpoint}/check/{WidgetDTO.Name}/{WidgetDTO.WidgetId}");

         WidgetExists = checkResponse.StatusCode.Equals(HttpStatusCode.Conflict);
    }
}