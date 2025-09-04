namespace ManufacturerManagerUI.Pages.Colours;

public partial class Edit
{
    protected override async Task OnInitializedAsync()
    {
        try
        {
            ColourDTO = await Http.GetFromJsonAsync<ColourDTO>($"{ColoursEndpoint}/{ColourId}") ?? new();
            MainLayout.SetHeaderValue("Edit Colour");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error fetching colour: " + ex.Message);
            Console.WriteLine("StackTrace: " + ex.StackTrace);
        }
    }

    protected override void OnInitialized()
    {
        base.OnInitialized();
        MainLayout.SetBreadCrumbs(
        [
            GetColourHomeBreadcrumbItem(),
            GetCustomBreadcrumbItem("Edit"),
        ]);
    }

    private async Task UpdateColour()
    {
        var response = await Http.PutAsJsonAsync($"{ColoursEndpoint}/{ColourId}", ColourDTO);

        if (response.StatusCode.Equals(HttpStatusCode.Conflict))
        {
            ColourExists = true;
            return;
        }

        if (response.IsSuccessStatusCode)
        {
            Snackbar.Add($"Colour {ColourDTO.Name} successfully updated.", Severity.Success);
            NavigationManager.NavigateTo("/colours/index");
        }
        else
        {
            var strResponse = await response.Content.ReadAsStringAsync();
            Snackbar.Add($"An error occurred updating Colour {ColourDTO.Name}. Please try again. The error message is {strResponse}.", Severity.Error);
        }
    }
}