
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
            MainLayout.SetHeaderValue("Edit Manufacturer");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error fetching manufacturer statuses: " + ex.Message);
            Console.WriteLine("StackTrace: " + ex.StackTrace);
        }
    }

    protected override void OnInitialized()
    {
        MainLayout.SetBreadCrumbs(
        [
            GetHomeBreadcrumbItem(),
            GetManufacturerHomeBreadcrumbItem(),
            GetCustomBreadcrumbItem("Edit"),
        ]);
    }

    private async Task UpdateManufacturer()
    {
        var response = await Http.PutAsJsonAsync($"{ManufacturersEndpoint}/{ManufacturerId}", ManufacturerDTO);
        if (response.IsSuccessStatusCode)
        {
            Snackbar.Add($"Manufacturer {ManufacturerDTO.Name} successfully updated.", Severity.Success);
            NavigationManager.NavigateTo("/manufacturers/index");
        }
        else
        {
            Snackbar.Add($"An error occurred updating Manufacturer {ManufacturerDTO.Name}. Please try again", Severity.Error);
            var strResponse = await response.Content.ReadAsStringAsync();
            Console.WriteLine("Json Response: \n " + strResponse);
        }
    }
}
