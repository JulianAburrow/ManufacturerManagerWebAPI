namespace ManufacturerManagerUI.Pages.Colours;

using MudBlazor;
using System.Net.Http.Json;

public partial class Create
    : ColourBasePageClass
{
    private async Task CreateColour()
    {
        var response = await Http.PostAsJsonAsync(GlobalValues.ColoursEndpoint, ColourDTO);

        if (response.IsSuccessStatusCode)
        {
            NavigationManager.NavigateTo("colours/index");
        }
        else
        {
            var strResponse = await response.Content.ReadAsStringAsync();
            Console.WriteLine("Json Response: \n " + strResponse);
        }
    }
}
