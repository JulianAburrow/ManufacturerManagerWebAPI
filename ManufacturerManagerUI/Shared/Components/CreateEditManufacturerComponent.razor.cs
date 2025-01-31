namespace ManufacturerManagerUI.Shared.Components;

public partial class CreateEditManufacturerComponent
{
    [Parameter]
    public ManufacturerDTO ManufacturerDTO { get; set; } = new();
    [Parameter]
    public List<ManufacturerStatusDTO> ManufacturerStatusDTOs { get; set; } = [];
}
