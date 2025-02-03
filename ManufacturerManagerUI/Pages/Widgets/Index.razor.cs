
using System.Net.Http.Json;

namespace ManufacturerManagerUI.Pages.Widgets;

public partial class Index
    : WidgetBasePageClass
{
    List<WidgetDTO>? Widgets = [];

    protected override async Task OnInitializedAsync()
    {
        try
        {
            Widgets = await Http.GetFromJsonAsync<List<WidgetDTO>>(WidgetsEndpoint) ?? [];
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error fetching widgets: " + ex.Message);
            Console.WriteLine("StackTrace: " + ex.StackTrace);
        }
    }
}
