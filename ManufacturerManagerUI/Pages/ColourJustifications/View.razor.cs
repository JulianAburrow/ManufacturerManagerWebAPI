using System.Net.Http.Json;

namespace ManufacturerManagerUI.Pages.ColourJustifications;

public partial class View(HttpClient http, NavigationManager NavigationManager)
    : ColourJustificationBasePageClass(http, NavigationManager)

{
    protected List<WidgetDTO> WidgetDTOs = [];

    protected override async Task OnInitializedAsync()
    {
        try
        {
            ColourJustificationDTO = await Http.GetFromJsonAsync<ColourJustificationDTO>($"{GlobalValues.ColourJustificationsEndpoint}/{ColourJustificationId}") ?? new();
            WidgetDTOs = await Http.GetFromJsonAsync<List<WidgetDTO>>($"{WidgetsEndpoint}/widgetsbycolourjustification/{ColourJustificationId}") ?? new();
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error fetching colour justification: " + ex.Message);
            Console.WriteLine("StackTrace: " + ex.StackTrace);
        }
    }

}
