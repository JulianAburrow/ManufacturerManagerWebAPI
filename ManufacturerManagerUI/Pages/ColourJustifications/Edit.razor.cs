using System.Net.Http.Json;

namespace ManufacturerManagerUI.Pages.ColourJustifications;

public partial class Edit
    : ColourJustificationBasePageClass
{
    protected override async Task OnInitializedAsync()
    {
        try
        {
            ColourJustificationDTO = await Http.GetFromJsonAsync<ColourJustificationDTO>($"{GlobalValues.ColourJustificationsEndpoint}/{ColourJustificationId}") ?? new();
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error fetching colour justification: " + ex.Message);
            Console.WriteLine("StackTrace: " + ex.StackTrace);
        }
    }

    private async Task UpdateColourJustification()
    {
        var response = await Http.PutAsJsonAsync<ColourJustificationDTO>($"{GlobalValues.ColourJustificationsEndpoint}/{ColourJustificationId}", ColourJustificationDTO);
        if (response.IsSuccessStatusCode)
        {
            NavigationManager.NavigateTo("/colourjustifications/index");
        }
        else
        {
            var strResponse = await response.Content.ReadAsStringAsync();
            Console.WriteLine("Json Response: \n " + strResponse);
        }
    }
}
