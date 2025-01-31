namespace ManufacturerManagerUI.Shared.BasePageClasses;

public class ColourBasePageClass(HttpClient http, NavigationManager navigationManager)
    : BasePageClass(http, navigationManager)
{
    [Parameter]
    public int ColourId { get; set; }

    protected ColourDTO ColourDTO = new();
}
