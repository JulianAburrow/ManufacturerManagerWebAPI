namespace ManufacturerManagerUI.Shared.BasePageClasses;

public class BasePageClass(HttpClient http, NavigationManager navigationManager) : ComponentBase
{
    protected readonly HttpClient Http = http;
    protected readonly NavigationManager NavigationManager = navigationManager;
    protected string WidgetsEndpoint { get; set; } = "api/Widget";
}
