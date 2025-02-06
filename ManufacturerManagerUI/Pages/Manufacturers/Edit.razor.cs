
using System.Net.Http.Json;

namespace ManufacturerManagerUI.Pages.Manufacturers;
public partial class Edit
{
    protected override async Task OnInitializedAsync()
    {
        try
        {
            ManufacturerDTO = await Http.GetFromJsonAsync<ManufacturerDTO>($"{ManufacturersEndpoint}/{ManufacturerId}") ?? new();
            ManufacturerStatusDTOs = await Http.GetFromJsonAsync<List<ManufacturerStatusDTO>>(ManufacturerStatusesEndpoint) ?? [];
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error fetching manufacturer statuses: " + ex.Message);
            Console.WriteLine("StackTrace: " + ex.StackTrace);
        }
    }
    private async Task UpdateManufacturer()
    {
        var response = await Http.PutAsJsonAsync($"{ManufacturersEndpoint}/{ManufacturerId}", ManufacturerDTO);
        if (response.IsSuccessStatusCode)
        {
            NavigationManager.NavigateTo("/manufacturers/index");
        }
        else
        {
            var strResponse = await response.Content.ReadAsStringAsync();
            Console.WriteLine("Json Response: \n " + strResponse);
        }
    }
}
