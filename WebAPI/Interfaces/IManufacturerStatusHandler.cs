namespace WebAPI.Interfaces;

public interface IManufacturerStatusHandler
{
    Task<List<ManufacturerStatusDTO>> GetManufacturerStatusesAsync();
}