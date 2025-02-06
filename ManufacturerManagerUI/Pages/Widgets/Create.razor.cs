namespace ManufacturerManagerUI.Pages.Widgets;

using System.Net.Http.Json;
using static ManufacturerManagerUI.GlobalValues;

public partial class Create
{
    protected override async Task OnInitializedAsync()
    {
        try
        {
            WidgetStatusDTOs = await Http.GetFromJsonAsync<List<WidgetStatusDTO>>(WidgetStatusesEndpoint) ?? [];
            WidgetStatusDTOs.Insert(0, SelectWidgetStatus);
            ManufacturerDTOs = await Http.GetFromJsonAsync<List<ManufacturerDTO>>(ManufacturersEndpoint) ?? [];
            ManufacturerDTOs.Insert(0, SelectManufacturer);
            ColourDTOs = await Http.GetFromJsonAsync<List<ColourDTO>>(ColoursEndpoint) ?? [];
            ColourDTOs.Insert(0, NoneColour);
            ColourJustificationDTOs = await Http.GetFromJsonAsync<List<ColourJustificationDTO>>(ColourJustificationsEndpoint) ?? [];
            ColourJustificationDTOs.Insert(0, NoneColourJustification);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error fetching information: " + ex.Message);
            Console.WriteLine("StackTrace: " + ex.StackTrace);
        }
    }

    private async Task CreateWidget()
    {
        if (WidgetDTO.ColourId == 0)
        {
            WidgetDTO.ColourId = null;
        }
        if (WidgetDTO.ColourJustificationId == 0 || WidgetDTO.ColourId == null)
        {
            WidgetDTO.ColourJustificationId = null;
        }
        var manufacturer = ManufacturerDTOs.FirstOrDefault(x => x.ManufacturerId == WidgetDTO.ManufacturerId);
        if (manufacturer?.StatusName == "Inactive")
        {
            WidgetDTO.StatusId = 2;
        }

        var response = await Http.PostAsJsonAsync(WidgetsEndpoint, WidgetDTO);

        if (response.IsSuccessStatusCode)
        {
            NavigationManager.NavigateTo("/widgets/index");
        }
        else
        {
            var strResponse = await response.Content.ReadAsStringAsync();
            Console.WriteLine("Json Response: \n " + strResponse);
        }
    }
}
