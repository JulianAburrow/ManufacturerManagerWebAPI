namespace ManufacturerManagerUI.Pages.Widgets;

public partial class View
{
    protected override async Task OnInitializedAsync()
    {
        try
        {
            WidgetDTO = await Http.GetFromJsonAsync<WidgetDTO>($"{WidgetsEndpoint}/{WidgetId}") ?? new();
            MainLayout.SetHeaderValue("View Widget");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error fetching widget: " + ex.Message);
            Console.WriteLine("StackTrace: " + ex.StackTrace);
        }
    }

    protected override void OnInitialized()
    {
        base.OnInitialized();
        MainLayout.SetBreadCrumbs(
        [
            GetWidgetHomeBreadcrumbItem(),
            GetCustomBreadcrumbItem(ViewTextForBreadcrumb),
        ]);
    }
}
