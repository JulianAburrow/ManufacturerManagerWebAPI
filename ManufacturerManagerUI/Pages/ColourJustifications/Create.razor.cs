namespace ManufacturerManagerUI.Pages.ColourJustifications;

public partial class Create
{
    protected override void OnInitialized()
    {
        ColourJustificationDTO = new();
        MainLayout.SetHeaderValue("Create Colour Justification");
        MainLayout.SetBreadCrumbs(
        [
            GetHomeBreadcrumbItem(),
            GetColourJustificationHomeBreadcrumbItem(),
            GetCustomBreadcrumbItem(CreateTextForBreadcrumb),
        ]);
    }

    private async Task CreateColourJustification()
    {
        var response = await Http.PostAsJsonAsync(GlobalValues.ColourJustificationsEndpoint, ColourJustificationDTO);
        if (response.IsSuccessStatusCode)
        {
            Snackbar.Add($"Colour Justification {ColourJustificationDTO.Justification} successfully created.", Severity.Success);
            NavigationManager.NavigateTo("colourjustifications/index");
        }
        else
        {
            Snackbar.Add($"An error occurred creating Colour Justification {ColourJustificationDTO.Justification}. Please try again", Severity.Error);
            var strResponse = await response.Content.ReadAsStringAsync();
            Console.WriteLine("Json Response: \n " + strResponse);
        }
    }
}
