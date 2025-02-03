namespace ManufacturerManagerUI.Pages;

public partial class Home
    : BasePageClass
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
