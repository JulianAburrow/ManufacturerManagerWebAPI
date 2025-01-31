namespace ManufacturerManagerUI.Shared.BasePageClasses;

public class ColourJustificationBasePageClass(HttpClient http, NavigationManager navigationManager)
    : BasePageClass(http, navigationManager)
{
    [Parameter]
    public int ColourJustificationId { get; set; }

    protected ColourJustificationDTO ColourJustificationDTO = new();
}
