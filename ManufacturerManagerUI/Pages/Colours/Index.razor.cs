namespace ManufacturerManagerUI.Pages.Colours;

public partial class Index
{
    List<ColourDTO> ColourDTOs = null!;

    protected override async Task OnInitializedAsync()
    {
        try
        {
            ColourDTOs = await Http.GetFromJsonAsync<List<ColourDTO>>(GlobalValues.ColoursEndpoint) ?? [];
            Snackbar.Add($"{ColourDTOs.Count} item(s) found.", ColourDTOs.Count > 0 ? Severity.Info : Severity.Warning);
            MainLayout.SetHeaderValue("Colours");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error fetching colours: " + ex.Message);
            Console.WriteLine("StackTrace: " + ex.StackTrace);
        }
    }

    protected override void OnInitialized()
    {
        MainLayout.SetBreadCrumbs(
        [
            GetHomeBreadcrumbItem(),
            GetCustomBreadcrumbItem("Colours")
        ]);
    }
}
