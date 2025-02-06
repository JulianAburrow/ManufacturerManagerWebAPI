using System.Net.Http.Json;

namespace ManufacturerManagerUI.Pages.ColourJustifications;

public partial class View

{
    protected List<WidgetDTO> WidgetDTOs = [];

    protected override async Task OnInitializedAsync()
    {
        try
        {
            ColourJustificationDTO = await Http.GetFromJsonAsync<ColourJustificationDTO>($"{GlobalValues.ColourJustificationsEndpoint}/{ColourJustificationId}") ?? new();
            WidgetDTOs = await Http.GetFromJsonAsync<List<WidgetDTO>>($"{WidgetsEndpoint}/widgetsbycolourjustification/{ColourJustificationId}") ?? new();
            MainLayout.SetHeaderValue("View Colour Justification");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error fetching colour justification: " + ex.Message);
            Console.WriteLine("StackTrace: " + ex.StackTrace);
        }
    }

    protected override void OnInitialized()
    {
        MainLayout.SetBreadCrumbs(
        [
            GetHomeBreadcrumbItem(),
            GetColourJustificationHomeBreadcrumbItem(),
            GetCustomBreadcrumbItem(ViewTextForBreadcrumb),
        ]);
    }
}
