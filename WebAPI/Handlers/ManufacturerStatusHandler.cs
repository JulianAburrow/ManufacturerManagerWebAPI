
namespace WebAPI.Handlers;

public class ManufacturerStatusHandler(ManufacturerManagerDbContext context) : IManufacturerStatusHandler
{
    private readonly ManufacturerManagerDbContext _context = context;

    public async Task<List<ManufacturerStatusDTO>> GetManufacturerStatusesAsync()
    {
        var manufacturerStatuses = await _context.ManufacturerStatuses
            .OrderBy(s => s.StatusName)
            .AsNoTracking()
            .ToListAsync();

        var manufacturerStatusDTOs = new List<ManufacturerStatusDTO>();

        foreach (var manufacturerStatus in manufacturerStatuses)
        {
            manufacturerStatusDTOs.Add(new ManufacturerStatusDTO
            {
                StatusId = manufacturerStatus.StatusId,
                StatusName = manufacturerStatus.StatusName,
            });
        }

        return manufacturerStatusDTOs;
    }
}
