

namespace ManufacturerManagerUI.Shared.BasePageClasses;

public class BasePageClass : ComponentBase
{
    [CascadingParameter] public MainLayout MainLayout { get; set; } = new();

    [Inject] protected ISnackbar Snackbar { get; set; } = default!;

    [Inject] protected HttpClient Http { get; set; } = default!;
    
    [Inject] protected NavigationManager NavigationManager { get; set; } = default!;
    protected string WidgetsEndpoint { get; set; } = "api/Widget";

    protected BreadcrumbItem GetHomeBreadcrumbItem(bool isDisabled = false)
    {
        return new("Home", "#", isDisabled, icon: Icons.Material.Filled.Home);
    }

    protected BreadcrumbItem GetCustomBreadcrumbItem(string text)
    {
        return new(text, null, true);
    }

    protected string CreateTextForBreadcrumb = "Create";

    protected string DeleteTextForBreadcrumb = "Delete";

    protected string EditTextForBreadcrumb = "Edit";

    protected string ViewTextForBreadcrumb = "View";


}
