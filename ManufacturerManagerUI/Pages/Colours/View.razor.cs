using System.Net.Http.Json;

namespace ManufacturerManagerUI.Pages.Colours;

public partial class View(HttpClient http, NavigationManager NavigationManager)
    : ColourBasePageClass(http, NavigationManager)
{
    List<WidgetDTO> WidgetDTOs = [];

    protected override async Task OnInitializedAsync()
    {
        try
        {
            ColourDTO = await Http.GetFromJsonAsync<ColourDTO>($"{GlobalValues.ColoursEndpoint}/{ColourId}") ?? new();
            WidgetDTOs = await Http.GetFromJsonAsync<List<WidgetDTO>>($"{WidgetsEndpoint}/widgetsbycolour/{ColourId}") ?? new();
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error fetching colour: " + ex.Message);
            Console.WriteLine("StackTrace: " + ex.StackTrace);
        }
    }
}
