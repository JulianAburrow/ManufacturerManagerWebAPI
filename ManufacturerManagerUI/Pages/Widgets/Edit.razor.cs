using System.Net.Http.Json;
using static ManufacturerManagerUI.GlobalValues;

namespace ManufacturerManagerUI.Pages.Widgets;

public partial class Edit
{
    protected override async Task OnInitializedAsync()
    {
        try
        {
            WidgetDTO = await Http.GetFromJsonAsync<WidgetDTO>($"{WidgetsEndpoint}/{WidgetId}") ?? new();
            WidgetDTO.ColourId ??= 0;
            WidgetDTO.ColourJustificationId ??= 0;
            WidgetStatusDTOs = await Http.GetFromJsonAsync<List<WidgetStatusDTO>>(WidgetStatusesEndpoint) ?? [];
            WidgetStatusDTOs.Insert(0, SelectWidgetStatus);
            ManufacturerDTOs = await Http.GetFromJsonAsync<List<ManufacturerDTO>>(ManufacturersEndpoint) ?? [];
            ManufacturerDTOs.Insert(0, SelectManufacturer);
            ColourDTOs = await Http.GetFromJsonAsync<List<ColourDTO>>(ColoursEndpoint) ?? [];
            ColourDTOs.Insert(0, NoneColour);
            ColourJustificationDTOs = await Http.GetFromJsonAsync<List<ColourJustificationDTO>>(ColourJustificationsEndpoint) ?? [];
            ColourJustificationDTOs.Insert(0, NoneColourJustification);
            MainLayout.SetHeaderValue("Edit Widget");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error fetching information: " + ex.Message);
            Console.WriteLine("StackTrace: " + ex.StackTrace);
        }
    }
    protected override void OnInitialized()
    {
        MainLayout.SetBreadCrumbs(
        [
            GetHomeBreadcrumbItem(),
            GetWidgetHomeBreadcrumbItem(),
            GetCustomBreadcrumbItem("Edit"),
        ]);
    }

    private async Task UpdateWidget()
    {
        if (WidgetDTO.ColourId == 0)
        {
            WidgetDTO.ColourId = null;
        }
        if (WidgetDTO.ColourJustificationId == 0 || WidgetDTO.ColourId == null)
        {
            WidgetDTO.ColourJustificationId = null;
        }

        var response = await Http.PutAsJsonAsync($"{WidgetsEndpoint}/{WidgetId}", WidgetDTO);
        if (response.IsSuccessStatusCode)
        {
            Snackbar.Add($"Widget {WidgetDTO.Name} successfully updated.", Severity.Success);
            NavigationManager.NavigateTo("/widgets/index");
        }
        else
        {
            Snackbar.Add($"An error occurred updating Widget {WidgetDTO.Name}. Please try again", Severity.Error);
            var strResponse = await response.Content.ReadAsStringAsync();
            Console.WriteLine("Json Response: \n " + strResponse);
        }
    }
}
