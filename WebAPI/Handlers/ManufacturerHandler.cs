namespace WebAPI.Handlers;

public class ManufacturerHandler(ManufacturerManagerDbContext context) : IManufacturerHandler
{
    private readonly ManufacturerManagerDbContext _context = context;

    public async Task<List<ManufacturerModel>> CheckForExistingManufacturerAsync(string manufacturerName, int id)
    {
        var manufacturers = await _context.Manufacturers
            .Where(
                m => m.Name.Replace(" ", "") == manufacturerName.Replace(" ", ""))
            .ToListAsync();

        if (id > 0)
        {
            manufacturers = [.. manufacturers.Where(m => m.ManufacturerId != id)];
        }

        return manufacturers;
    }

    public async Task<ActionResult> CreateManufacturerAsync(ManufacturerDTO manufacturerDTO)
    {
        var manufacturer = new ManufacturerModel
        {
            Name = manufacturerDTO.Name,
            StatusId = manufacturerDTO.StatusId,
        };

        try
        {
            _context.Manufacturers.Add(manufacturer);
            await _context.SaveChangesAsync();
            manufacturerDTO.ManufacturerId = manufacturer.ManufacturerId;
            return new CreatedResult($"/api/manufacturers/{manufacturer.ManufacturerId}", manufacturerDTO);
        }
        catch (Exception ex)
        {
            return new BadRequestObjectResult($"An error occurred while creating the Manufacturer: {ex.Message}");
        }
    }

    public async Task<ManufacturerDTO>? GetManufacturerAsync(int id)
    {
        var manufacturer = await _context.Manufacturers
            .Include(m => m.Widgets)
            .Include(m => m.Status)
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.ManufacturerId == id);

        if (manufacturer is null)
        {
            return null;
        }

        var manufacturerDTO = new ManufacturerDTO
        {
            ManufacturerId = manufacturer.ManufacturerId,
            Name = manufacturer.Name,
            StatusId = manufacturer.StatusId,
            StatusName = manufacturer.Status.StatusName,
            WidgetCount = manufacturer.Widgets.Count,
        };

        return manufacturerDTO;
    }

    public async Task<List<ManufacturerDTO>> GetManufacturersAsync()
    {
        var manufacturers = await _context.Manufacturers
            .Include(m => m.Widgets)
            .Include(m => m.Status)
            .OrderBy(m => m.Name)
            .AsNoTracking()
            .ToListAsync();

        var manufacturerDTOs = new List<ManufacturerDTO>();

        foreach (var manufacturer in manufacturers)
        {
            manufacturerDTOs.Add(new ManufacturerDTO
            {
                ManufacturerId = manufacturer.ManufacturerId,
                Name = manufacturer.Name,
                StatusId = manufacturer.StatusId,
                StatusName = manufacturer.Status.StatusName,
                WidgetCount = manufacturer.Widgets.Count,
            });
        }

        return manufacturerDTOs;
    }

    public async Task<ActionResult> UpdateManufacturerAsync(int id, ManufacturerDTO manufacturerDTO)
    {
        var manufacturerToUpdate = await _context.Manufacturers
            .FindAsync(id);

        if (manufacturerToUpdate is null)
        {
            return new NotFoundObjectResult("No Manufacturer with this id could be found");
        }

        manufacturerToUpdate.Name = manufacturerDTO.Name;
        manufacturerToUpdate.StatusId = manufacturerDTO.StatusId;

        if (manufacturerToUpdate.StatusId == (int) StatusesEnum.Inactive)
        {
            var widgets = _context.Widgets
                .Where(w => w.ManufacturerId == manufacturerToUpdate.ManufacturerId);
            foreach (var widget in widgets)
            {
                widget.StatusId = (int) StatusesEnum.Inactive;
            }
        }

        try
        {
            await _context.SaveChangesAsync();
            return new NoContentResult();
        }
        catch (Exception ex)
        {
            return new BadRequestObjectResult($"An error occurred while updating the Manufacturer: {ex.Message}");
        }
    }
}
