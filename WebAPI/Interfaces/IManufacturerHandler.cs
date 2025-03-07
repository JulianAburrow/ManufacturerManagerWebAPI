namespace WebAPI.Interfaces;

public interface IManufacturerHandler
{
    Task<List<ManufacturerDTO>> GetManufacturersAsync();

    Task<ManufacturerDTO>? GetManufacturerAsync(int id);

    Task<ActionResult> CreateManufacturerAsync(ManufacturerDTO manufacturerDTO);

    Task<ActionResult> UpdateManufacturerAsync(int id, ManufacturerDTO manufacturerDTO);
}
