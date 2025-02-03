using MudBlazor;

namespace ManufacturerManagerUI.Shared.BasePageClasses;

public class ColourBasePageClass
    : BasePageClass
{
    [Parameter]
    public int ColourId { get; set; }

    protected ColourDTO ColourDTO = new();
}
