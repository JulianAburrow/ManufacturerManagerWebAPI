namespace ManufacturerManagerUI.Pages.Manufacturers;

using System.Net.Http.Json;

public partial class Create
{
    protected override async Task OnInitializedAsync()
    {
        try
        {
            ManufacturerDTO = new ManufacturerDTO
            {
                StatusId = GlobalValues.SelectValue,
            };
            ManufacturerStatusDTOs = await Http.GetFromJsonAsync<List<ManufacturerStatusDTO>>(ManufacturerStatusesEndpoint) ?? [];
            ManufacturerStatusDTOs.Insert(0, SelectManufacturerStatus);
            MainLayout.SetHeaderValue("Create Manufacturer");
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
            GetCustomBreadcrumbItem("Create Manufacturer")
        ]);
    }

    private async Task CreateManufacturer()
    {
        await CheckForExistingManufacturer();

        if (ManufacturerExists)
        {
            return;
        }    

        var response = await Http.PostAsJsonAsync(ManufacturersEndpoint, ManufacturerDTO);

        if (response.IsSuccessStatusCode)
        {
            Snackbar.Add($"Manufacturer {ManufacturerDTO.Name} successfully created.", Severity.Success);
            NavigationManager.NavigateTo("/manufacturers/index");
        }
        else
        {
            Snackbar.Add($" An error occurred creating Manufacturer {ManufacturerDTO.Name}. Please try again.", Severity.Error);
            var strResponse = await response.Content.ReadAsStringAsync();
            Console.WriteLine("Json Response: \n " + strResponse);
        }
    }
}
