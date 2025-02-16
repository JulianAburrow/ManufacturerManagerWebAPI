namespace ManufacturerManagerUI.Pages.Colours;

public partial class Create
{
    protected override void OnInitialized()
    {
        ColourDTO = new();
        MainLayout.SetHeaderValue("Create Colour");
        MainLayout.SetBreadCrumbs(
        [
            GetHomeBreadcrumbItem(),
            GetColourHomeBreadcrumbItem(),
            GetCustomBreadcrumbItem(CreateTextForBreadcrumb),
        ]);
    }

    private async Task CreateColour()
    {
        var response = await Http.PostAsJsonAsync(GlobalValues.ColoursEndpoint, ColourDTO);

        if (response.StatusCode.Equals(HttpStatusCode.Conflict))
        {
            ColourExists = true;
            return;
        }

        if (response.IsSuccessStatusCode)
        {
            Snackbar.Add($"Colour {ColourDTO.Name} successfully created.", Severity.Success);
            NavigationManager.NavigateTo("colours/index");
        }
        else
        {
            var strResponse = await response.Content.ReadAsStringAsync();
            Console.WriteLine("Json Response: \n " + strResponse);
        }
    }
}
