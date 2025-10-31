namespace ManufacturerManagerUI.Pages;

public partial class Home
{
    protected override void OnInitialized()
    {
        base.OnInitialized();
        MainLayout.SetHeaderValue("Home");
        MainLayout.SetBreadCrumbs(
        [
            GetHomeBreadcrumbItem(true),
        ]);
    }
}
