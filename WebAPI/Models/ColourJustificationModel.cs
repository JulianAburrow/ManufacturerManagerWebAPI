namespace WebAPI.Models;

public class ColourJustificationModel
{
    public int ColourJustificationId {  get; set; }

    public string Justification { get; set; } = default!;

    public ICollection<WidgetModel> Widgets { get; set; } = null!;
}
