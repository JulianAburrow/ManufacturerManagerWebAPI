using System.Net.Http.Json;

namespace ManufacturerManagerUI.Pages.Manufacturers;

public partial class Index
{
    List<ManufacturerDTO>? Manufacturers = [];

    protected override async Task OnInitializedAsync()
    {
        try
        {
            Manufacturers = await Http.GetFromJsonAsync<List<ManufacturerDTO>>(ManufacturersEndpoint) ?? [];
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error fetching manufacturers: " + ex.Message);
            Console.WriteLine("StackTrace: " + ex.StackTrace);
        }
    }
}
