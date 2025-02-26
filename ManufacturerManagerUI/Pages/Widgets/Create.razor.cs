namespace ManufacturerManagerUI.Pages.Widgets;

public partial class Create
{
    protected override async Task OnInitializedAsync()
    {
        try
        {
            WidgetDTO = new WidgetDTO
            {
                ManufacturerId = GlobalValues.SelectValue,
                StatusId = GlobalValues.SelectValue,
                ColourId = GlobalValues.NoneValue,
                ColourJustificationId = GlobalValues.NoneValue,
            };
            WidgetStatusDTOs = await Http.GetFromJsonAsync<List<WidgetStatusDTO>>(WidgetStatusesEndpoint) ?? [];
            WidgetStatusDTOs.Insert(0, SelectWidgetStatus);
            ManufacturerDTOs = await Http.GetFromJsonAsync<List<ManufacturerDTO>>(GlobalValues.ManufacturersEndpoint) ?? [];
            ManufacturerDTOs.Insert(0, SelectManufacturer);
            ColourDTOs = await Http.GetFromJsonAsync<List<ColourDTO>>(GlobalValues.ColoursEndpoint) ?? [];
            ColourDTOs.Insert(0, NoneColour);
            ColourJustificationDTOs = await Http.GetFromJsonAsync<List<ColourJustificationDTO>>(GlobalValues.ColourJustificationsEndpoint) ?? [];
            ColourJustificationDTOs.Insert(0, NoneColourJustification);
            MainLayout.SetHeaderValue("Create Widget");
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
            GetCustomBreadcrumbItem("Create Widget"),
        ]);
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

        await CheckForExistingWidget();

        if (WidgetExists)
        {
            return;
        }

        var response = await Http.PostAsJsonAsync(WidgetsEndpoint, WidgetDTO);

        if (response.IsSuccessStatusCode)
        {
            Snackbar.Add($"Widget {WidgetDTO.Name} successfully created.", Severity.Success);
            NavigationManager.NavigateTo("/widgets/index");
        }
        else
        {
            Snackbar.Add($"An error occurred creating Widget {WidgetDTO.Name}. Please try again", Severity.Error);
            var strResponse = await response.Content.ReadAsStringAsync();
            Console.WriteLine("Json Response: \n " + strResponse);
        }
    }
}
