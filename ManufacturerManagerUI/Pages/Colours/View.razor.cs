namespace ManufacturerManagerUI.Pages.Colours;

public partial class View
{
    List<WidgetDTO> WidgetDTOs = [];

    protected override async Task OnInitializedAsync()
    {
        try
        {
            ColourDTO = await Http.GetFromJsonAsync<ColourDTO>($"{ColoursEndpoint}/{ColourId}") ?? new();
            WidgetDTOs = await Http.GetFromJsonAsync<List<WidgetDTO>>($"{WidgetsEndpoint}/widgetsbycolour/{ColourId}") ?? new();
            MainLayout.SetHeaderValue("View Colour");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error fetching colour: " + ex.Message);
            Console.WriteLine("StackTrace: " + ex.StackTrace);
        }
    }

    protected override void OnInitialized()
    {
        base.OnInitialized();
        MainLayout.SetBreadCrumbs(
        [
            GetColourHomeBreadcrumbItem(),
            GetCustomBreadcrumbItem(ViewTextForBreadcrumb),
        ]);
    }
}
