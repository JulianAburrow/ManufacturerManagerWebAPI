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

        if (response.IsSuccessStatusCode)
        {
            NavigationManager.NavigateTo("colours/index");
        }
        else
        {
            var strResponse = await response.Content.ReadAsStringAsync();
            Console.WriteLine("Json Response: \n " + strResponse);
        }
    }
}
