namespace ManufacturerManagerUI.Shared.Components;

public partial class DisplayWidgetListComponent
{
    [Parameter]
    public List<WidgetDTO> WidgetDTOs { get; set; } = [];
}
