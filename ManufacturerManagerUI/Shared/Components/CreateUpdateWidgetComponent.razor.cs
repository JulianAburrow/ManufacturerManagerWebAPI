namespace ManufacturerManagerUI.Shared.Components;

public partial class CreateUpdateWidgetComponent()
{
    [Parameter]
    public WidgetDTO WidgetDTO { get; set;} = new();
    [Parameter]
    public List<ManufacturerDTO> ManufacturerDTOs { get; set; } = [];
    [Parameter]
    public List<ColourDTO> ColourDTOs { get; set; } = [];
    [Parameter]
    public List<ColourJustificationDTO> ColourJustificationDTOs { get; set; } = [];
    [Parameter]
    public List<WidgetStatusDTO> WidgetStatusDTOs { get; set; } = [];

    private bool ShowWidgetWillBeInactiveWarning;

    protected override void OnInitialized()
    {
        CheckManufacturerStatus();
    }

    private void CheckManufacturerStatus()
    {
        var manufacturer = ManufacturerDTOs.FirstOrDefault(x => x.ManufacturerId == WidgetDTO.ManufacturerId);
        ShowWidgetWillBeInactiveWarning = manufacturer?.StatusName == "Inactive";
        if (ShowWidgetWillBeInactiveWarning)
        {
            WidgetDTO.StatusId = 2;
        }   
    }
}
