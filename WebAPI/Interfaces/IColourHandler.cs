namespace WebAPI.Interfaces;

public interface IColourHandler
{
    Task<List<ColourDTO>> GetColoursAsync();

    Task<ColourDTO>? GetColourAsync(int id);

    Task<ActionResult> CreateColourAsync(ColourDTO colourDTO);

    Task<ActionResult> UpdateColourAsync(int id, ColourDTO colourDTO);
}
