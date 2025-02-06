namespace ManufacturerManagerUI.Pages;

public partial class Home
{
    protected override void OnInitialized()
    {
        MainLayout.SetHeaderValue("Home");
        MainLayout.SetBreadCrumbs(
        [
            GetHomeBreadcrumbItem(true)
        ]);
    }
}
