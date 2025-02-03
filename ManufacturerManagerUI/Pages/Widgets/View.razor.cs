using System.Net.Http.Json;

namespace ManufacturerManagerUI.Pages.Widgets;

public partial class View
    : WidgetBasePageClass
{
    protected override async Task OnInitializedAsync()
    {
        try
        {
            WidgetDTO = await Http.GetFromJsonAsync<WidgetDTO>($"{WidgetsEndpoint}/{WidgetId}") ?? new();
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error fetching widget: " + ex.Message);
            Console.WriteLine("StackTrace: " + ex.StackTrace);
        }
    }
}
