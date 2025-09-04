namespace ManufacturerManagerUI.Pages.Widgets;

public partial class Create
{
    protected override async Task OnInitializedAsync()
    {
        try
        {
            WidgetDTO = new WidgetDTO
            {
                ManufacturerId = SelectValue,
                StatusId = SelectValue,
                ColourId = NoneValue,
                ColourJustificationId = NoneValue,
            };
            WidgetStatusDTOs = await Http.GetFromJsonAsync<List<WidgetStatusDTO>>(WidgetStatusesEndpoint) ?? [];
            WidgetStatusDTOs.Insert(0, SelectWidgetStatus);
            ManufacturerDTOs = await Http.GetFromJsonAsync<List<ManufacturerDTO>>(ManufacturersEndpoint) ?? [];
            ManufacturerDTOs.Insert(0, SelectManufacturer);
            ColourDTOs = await Http.GetFromJsonAsync<List<ColourDTO>>(ColoursEndpoint) ?? [];
            ColourDTOs.Insert(0, NoneColour);
            ColourJustificationDTOs = await Http.GetFromJsonAsync<List<ColourJustificationDTO>>(ColourJustificationsEndpoint) ?? [];
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
        base.OnInitialized();
        MainLayout.SetBreadCrumbs(
        [
            GetWidgetHomeBreadcrumbItem(),
            GetCustomBreadcrumbItem("Create Widget"),
        ]);
    }

    private async Task CreateWidget()
    {

        await CheckForExistingWidget();

        if (WidgetExists)
        {
            return;
        }

        if (WidgetDTO.ColourId == 0)
        {
            WidgetDTO.ColourId = null;
        }
        if (WidgetDTO.ColourJustificationId == 0 || WidgetDTO.ColourId == null)
        {
            WidgetDTO.ColourJustificationId = null;
        }
        var manufacturer = ManufacturerDTOs.FirstOrDefault(x => x.ManufacturerId == WidgetDTO.ManufacturerId);
        if (manufacturer?.StatusName == StatusesEnum.Inactive.ToString())
        {
            WidgetDTO.StatusId = (int) StatusesEnum.Inactive;
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
