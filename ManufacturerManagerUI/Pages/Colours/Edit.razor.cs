namespace ManufacturerManagerUI.Pages.Colours;

using System.Net.Http.Json;

public partial class Edit
{
    protected override async Task OnInitializedAsync()
    {
        try
        {
            ColourDTO = await Http.GetFromJsonAsync<ColourDTO>($"{GlobalValues.ColoursEndpoint}/{ColourId}") ?? new();
            MainLayout.SetHeaderValue("Edit Colour");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error fetching colour: " + ex.Message);
            Console.WriteLine("StackTrace: " + ex.StackTrace);
        }
    }

    protected override void OnInitialized()
    {
        MainLayout.SetBreadCrumbs(
        [
            GetHomeBreadcrumbItem(),
            GetColourHomeBreadcrumbItem(),
            GetCustomBreadcrumbItem("Edit"),
        ]);
    }

    private async Task UpdateColour()
    {
        var response = await Http.PutAsJsonAsync($"{GlobalValues.ColoursEndpoint}/{ColourId}", ColourDTO);

        if (response.StatusCode.Equals(HttpStatusCode.Conflict))
        {
            ColourExists = true;
            return;
        }

        if (response.IsSuccessStatusCode)
        {
            Snackbar.Add($"Colour {ColourDTO.Name} successfully updated.", Severity.Success);
            NavigationManager.NavigateTo("/colours/index");
        }
        else
        {
            Snackbar.Add($"An error occurred updating Colour {ColourDTO.Name}. Please try again", Severity.Error);
            var strResponse = await response.Content.ReadAsStringAsync();
            Console.WriteLine("Json Response: \n " + strResponse);
        }
    }
}