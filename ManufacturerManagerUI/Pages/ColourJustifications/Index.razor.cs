using System.Net.Http.Json;

namespace ManufacturerManagerUI.Pages.ColourJustifications;

public partial class Index(HttpClient http)
{
    List<ColourJustificationDTO> ColourJustifications = [];
    protected readonly HttpClient Http = http;

    protected override async Task OnInitializedAsync()
    {
        try
        {
            ColourJustifications = await http.GetFromJsonAsync<List<ColourJustificationDTO>>(GlobalValues.ColourJustificationsEndpoint) ?? [];
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error fetching colourjustifications: " + ex.Message);
            Console.WriteLine("StackTrace: " + ex.StackTrace);
        }
    }
}
