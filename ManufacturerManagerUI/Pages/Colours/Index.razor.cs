using System.Net.Http.Json;

namespace ManufacturerManagerUI.Pages.Colours;

public partial class Index
{
    List<ColourDTO> Colours = null!;

    protected override async Task OnInitializedAsync()
    {
        try
        {
            Colours = await Http.GetFromJsonAsync<List<ColourDTO>>(GlobalValues.ColoursEndpoint) ?? [];
            Snackbar.Add($"{Colours.Count} item(s) found.", Colours.Count > 0 ? Severity.Info : Severity.Warning);
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
