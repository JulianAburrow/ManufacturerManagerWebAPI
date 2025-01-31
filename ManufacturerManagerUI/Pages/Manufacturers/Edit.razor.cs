namespace ManufacturerManagerUI.Pages.Manufacturers;

using System.Net.Http.Json;
using static ManufacturerManagerUI.GlobalValues;

public partial class Edit(HttpClient http, NavigationManager navigationManager)
    : ManufacturerBasePageClass(http, navigationManager)
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
