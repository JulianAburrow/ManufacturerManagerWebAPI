﻿namespace ManufacturerManagerUI.Pages.Manufacturers;

public partial class View
{
    List<WidgetDTO> WidgetDTOs = [];

    protected override async Task OnInitializedAsync()
    {
        try
        {
            ManufacturerDTO = await Http.GetFromJsonAsync<ManufacturerDTO>($"{ManufacturersEndpoint}/{ManufacturerId}") ?? new();
            WidgetDTOs = await Http.GetFromJsonAsync<List<WidgetDTO>>($"{WidgetsEndpoint}/widgetsbymanufacturer/{ManufacturerId}") ?? new();
            MainLayout.SetHeaderValue("View Manufacturer");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error fetching manufacturer: " + ex.Message);
            Console.WriteLine("StackTrace: " + ex.StackTrace);
        }
    }

    protected override void OnInitialized()
    {
        MainLayout.SetBreadCrumbs(
        [
            GetHomeBreadcrumbItem(),
            GetManufacturerHomeBreadcrumbItem(),
            GetCustomBreadcrumbItem(ViewTextForBreadcrumb),
        ]);
    }
}
