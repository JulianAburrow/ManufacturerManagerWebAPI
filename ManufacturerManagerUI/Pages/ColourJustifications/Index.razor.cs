namespace ManufacturerManagerUI.Pages.ColourJustifications;

public partial class Index
{
    List<ColourJustificationDTO> ColourJustificationDTOs { get; set; } = null!;
    
    protected override async Task OnInitializedAsync()
    {
        try
        {
            ColourJustificationDTOs = await Http.GetFromJsonAsync<List<ColourJustificationDTO>>(ColourJustificationsEndpoint) ?? [];
            Snackbar.Add($"{ColourJustificationDTOs.Count} item(s) found.", ColourJustificationDTOs.Count > 0 ? Severity.Info : Severity.Warning);
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
        base.OnInitialized();
        MainLayout.SetBreadCrumbs(
        [
            GetHomeBreadcrumbItem(),
            GetCustomBreadcrumbItem("Colour Justifications")
        ]);
    }
}
