namespace ManufacturerManagerUI.Pages.ColourJustifications;

public partial class Edit
{
    protected override async Task OnInitializedAsync()
    {
        try
        {
            ColourJustificationDTO = await Http.GetFromJsonAsync<ColourJustificationDTO>($"{ColourJustificationsEndpoint}/{ColourJustificationId}") ?? new();
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
        base.OnInitialized();
        MainLayout.SetBreadCrumbs(
        [
            GetHomeBreadcrumbItem(),
            GetColourJustificationHomeBreadcrumbItem(),
            GetCustomBreadcrumbItem("Edit"),
        ]);
    }

    private async Task UpdateColourJustification()
    {
        var response = await Http.PutAsJsonAsync($"{ColourJustificationsEndpoint}/{ColourJustificationId}", ColourJustificationDTO);

        if (response.StatusCode.Equals(HttpStatusCode.Conflict))
        {
            ColourJustificationExists = true;
            return;
        }

        if (response.IsSuccessStatusCode)
        {
            Snackbar.Add($"Colour Justification {ColourJustificationDTO.Justification} successfully updated.", Severity.Success);
            NavigationManager.NavigateTo("/colourjustifications/index");
        }
        else
        {
            var strResponse = await response.Content.ReadAsStringAsync();
            Snackbar.Add($"An error occurred updating Colour Justification {ColourJustificationDTO.Justification}. Please try again. The error message is {strResponse}.", Severity.Error);
        }
    }
}
