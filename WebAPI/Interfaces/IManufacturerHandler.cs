namespace WebAPI.Interfaces;

public interface IManufacturerHandler
{
    Task<List<ManufacturerModel>> CheckForExistingManufacturerAsync(string manufacturerName, int id);

    Task<List<ManufacturerDTO>> GetManufacturersAsync();

    Task<ManufacturerDTO>? GetManufacturerAsync(int id);

    Task<ActionResult> CreateManufacturerAsync(ManufacturerDTO manufacturerDTO);

    Task<ActionResult> UpdateManufacturerAsync(int id, ManufacturerDTO manufacturerDTO);
}
