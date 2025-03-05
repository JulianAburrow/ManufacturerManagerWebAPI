namespace WebAPI.Interfaces;

public interface IColourJustificationHandler
{
    Task<List<ColourJustificationDTO>> GetColourJustificationsAsync();

    Task<ColourJustificationDTO>? GetColourJustificationAsync(int id);

    Task<ActionResult> CreateColourJustificationAsync(ColourJustificationDTO colourJustificationDTO);

    Task<ActionResult> UpdateColourJustificationAsync(int id, ColourJustificationDTO colourJustificationDTO);
}
