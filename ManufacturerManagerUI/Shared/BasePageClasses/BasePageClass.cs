

namespace ManufacturerManagerUI.Shared.BasePageClasses;

public class BasePageClass : ComponentBase
{
    [CascadingParameter] public MainLayout MainLayout { get; set; } = new();

    [Inject] protected ISnackbar Snackbar { get; set; } = default!;

    [Inject] protected HttpClient Http { get; set; } = default!;

    [Inject] protected IConfiguration Configuration { get; set; } = default!;

    protected override void OnInitialized()
    {
        //var apiKeyName = Configuration["ApiSettings:ApiKeyName"];
        //var apiToken = Configuration["ApiSettings:ApiToken"];
        var apiKeyName = "X-Api-Key";
        var apiToken = "your-secret-api-key";
        if (string.IsNullOrWhiteSpace(apiKeyName) || string.IsNullOrWhiteSpace(apiToken))
        {
            throw new InvalidOperationException("ApiKeyName or ApiToken is not configured in appsettings.json");
        }

        if (Http.DefaultRequestHeaders.Contains(apiKeyName))
        {
            Http.DefaultRequestHeaders.Remove(apiKeyName);
        }
        Http.DefaultRequestHeaders.Add(apiKeyName, apiToken);
    }
    
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
