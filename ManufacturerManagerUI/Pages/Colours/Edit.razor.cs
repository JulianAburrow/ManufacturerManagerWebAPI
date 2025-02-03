namespace ManufacturerManagerUI.Pages.Colours;

using System.Net.Http.Json;

public partial class Edit
    : ColourBasePageClass
{
    protected override async Task OnInitializedAsync()
    {
        try
        {
            ColourDTO = await Http.GetFromJsonAsync<ColourDTO>($"{GlobalValues.ColoursEndpoint}/{ColourId}") ?? new();
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error fetching colour: " + ex.Message);
            Console.WriteLine("StackTrace: " + ex.StackTrace);
        }
    }

    private async Task UpdateColour()
    {
        var response = await Http.PutAsJsonAsync($"{GlobalValues.ColoursEndpoint}/{ColourId}", ColourDTO);
        if (response.IsSuccessStatusCode)
        {
            NavigationManager.NavigateTo("/colours/index");
        }
        else
        {
            var strResponse = await response.Content.ReadAsStringAsync();
            Console.WriteLine("Json Response: \n " + strResponse);
        }
    }
}