using System.Net.Http.Json;

namespace ManufacturerManagerUI.Pages.Colours;

public partial class Index(HttpClient http)
{
    List<ColourDTO> Colours = [];
    protected readonly HttpClient Http = http;

    protected override async Task OnInitializedAsync()
    {
        try
        {
            Colours = await http.GetFromJsonAsync<List<ColourDTO>>(GlobalValues.ColoursEndpoint) ?? [];
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error fetching colours: " + ex.Message);
            Console.WriteLine("StackTrace: " + ex.StackTrace);
        }
    }
}
