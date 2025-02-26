namespace ManufacturerManagerUI.Pages.Widgets;

public partial class Index
{
    List<WidgetDTO> WidgetDTOs { get; set; } = null!;

    protected override async Task OnInitializedAsync()
    {
        try
        {
            WidgetDTOs = await Http.GetFromJsonAsync<List<WidgetDTO>>(WidgetsEndpoint) ?? [];
            Snackbar.Add($"{WidgetDTOs.Count} item(s) found.", WidgetDTOs.Count > 0 ? Severity.Info : Severity.Warning);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error fetching widgets: " + ex.Message);
            Console.WriteLine("StackTrace: " + ex.StackTrace);
        }
    }

    protected override void OnInitialized()
    {
        MainLayout.SetBreadCrumbs(
        [
            GetHomeBreadcrumbItem(),
            GetCustomBreadcrumbItem("Widgets")
        ]);
    }
}
