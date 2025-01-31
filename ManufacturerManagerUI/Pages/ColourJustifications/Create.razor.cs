namespace ManufacturerManagerUI.Pages.ColourJustifications;

public partial class Create(HttpClient http, NavigationManager NavigationManager)
    : ColourJustificationBasePageClass(http, NavigationManager)
{
    private async Task CreateColourJustification()
    {
        var response = await Http.PostAsJsonAsync(GlobalValues.ColourJustificationsEndpoint, ColourJustificationDTO);
        if (response.IsSuccessStatusCode)
        {
            NavigationManager.NavigateTo("colourjustifications/index");
        }
        else
        {
            var strResponse = await response.Content.ReadAsStringAsync();
            Console.WriteLine("Json Response: \n " + strResponse);
        }
    }
}
