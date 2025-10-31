namespace ManufacturerManagerUI.Pages.Manufacturers;

public partial class Index
{
    List<ManufacturerDTO> ManufacturerDTOs { get; set; } = null!;

    protected override async Task OnInitializedAsync()
    {
        try
        {
            ManufacturerDTOs = await Http.GetFromJsonAsync<List<ManufacturerDTO>>(ManufacturersEndpoint) ?? [];
            Snackbar.Add($"{ManufacturerDTOs.Count} item(s) found.", ManufacturerDTOs.Count > 0 ? Severity.Info : Severity.Warning);
            MainLayout.SetHeaderValue("Manufacturers");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error fetching manufacturers: " + ex.Message);
            Console.WriteLine("StackTrace: " + ex.StackTrace);
        }
    }

    protected override void OnInitialized()
    {
        base.OnInitialized();
        MainLayout.SetBreadCrumbs(
        [
            GetHomeBreadcrumbItem(),
            GetCustomBreadcrumbItem("Manufacturers")
        ]);
    }
}
