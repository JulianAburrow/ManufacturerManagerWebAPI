using System.Net.Http.Json;

namespace ManufacturerManagerUI.Pages.ColourJustifications;

public partial class Edit
{
    protected override async Task OnInitializedAsync()
    {
        try
        {
            ColourJustificationDTO = await Http.GetFromJsonAsync<ColourJustificationDTO>($"{GlobalValues.ColourJustificationsEndpoint}/{ColourJustificationId}") ?? new();
            MainLayout.SetHeaderValue("Edit Colour Justification");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error fetching colour justification: " + ex.Message);
            Console.WriteLine("StackTrace: " + ex.StackTrace);
        }
    }

    protected override void OnInitialized()
    {
        MainLayout.SetBreadCrumbs(
        [
            GetHomeBreadcrumbItem(),
            GetColourJustificationHomeBreadcrumbItem(),
            GetCustomBreadcrumbItem("Edit"),
        ]);
    }

    private async Task UpdateColourJustification()
    {
        var response = await Http.PutAsJsonAsync($"{GlobalValues.ColourJustificationsEndpoint}/{ColourJustificationId}", ColourJustificationDTO);
        if (response.IsSuccessStatusCode)
        {
            Snackbar.Add($"Colour Justification {ColourJustificationDTO.Justification} successfully updated.", Severity.Success);
            NavigationManager.NavigateTo("/colourjustifications/index");
        }
        else
        {
            Snackbar.Add($"An error occurred updating Colour Justification {ColourJustificationDTO.Justification}. Please try again", Severity.Error);
            var strResponse = await response.Content.ReadAsStringAsync();
            Console.WriteLine("Json Response: \n " + strResponse);
        }
    }
}
