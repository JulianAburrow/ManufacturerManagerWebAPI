using MudBlazor;

namespace ManufacturerManagerUI.Shared.BasePageClasses;

public class ColourJustificationBasePageClass
    : BasePageClass
{
    [Parameter]
    public int ColourJustificationId { get; set; }

    protected ColourJustificationDTO ColourJustificationDTO = null!;

    protected BreadcrumbItem GetColourJustificationHomeBreadcrumbItem(bool isDisabled = false)
    {
        return new("ColourJustifications", "/colourjustifications/index", isDisabled);
    }
}
