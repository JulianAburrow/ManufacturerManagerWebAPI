using System.Net.Http.Json;

namespace ManufacturerManagerUI.Pages.ColourJustifications;

public partial class Index
    : ColourJustificationBasePageClass
{
    List<ColourJustificationDTO> ColourJustifications { get; set; } = null!;
    
    protected override async Task OnInitializedAsync()
    {
        try
        {
            ColourJustifications = await Http.GetFromJsonAsync<List<ColourJustificationDTO>>(GlobalValues.ColourJustificationsEndpoint) ?? [];
            Snackbar.Add($"{ColourJustifications.Count} item(s) found.", ColourJustifications.Count > 0 ? Severity.Info : Severity.Warning);
            MainLayout.SetHeaderValue("Colour Justifications");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error fetching colourjustifications: " + ex.Message);
            Console.WriteLine("StackTrace: " + ex.StackTrace);
        }
    }

    protected override void OnInitialized()
    {
        MainLayout.SetBreadCrumbs(
        [
            GetHomeBreadcrumbItem(),
                GetCustomBreadcrumbItem("Colour Justifications")
        ]);
    }
}
