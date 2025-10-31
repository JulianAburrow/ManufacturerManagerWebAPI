

namespace WebAPI.Handlers;

public class WidgetHandler(ManufacturerManagerDbContext context) : IWidgetHandler
{
    private readonly ManufacturerManagerDbContext _context = context;

    public async Task<List<WidgetModel>> CheckForExistingWidgetAsync(string widgetName, int id)
    {
        var widgets = await _context.Widgets
            .Where(
                w =>
                    w.Name.Replace(" ", "") == widgetName.Replace(" ", ""))
            .ToListAsync();

        if (id > 0)
        {
            widgets = [.. widgets.Where(w => w.WidgetId != id)];
        }

        return widgets;
    }

    public async Task<ActionResult> CreateWidgetAsync(WidgetDTO widgetDTO)
    {
        var widget = new WidgetModel
        {
            Name = widgetDTO.Name,
            ManufacturerId = widgetDTO.ManufacturerId,
            ColourId = widgetDTO.ColourId,
            ColourJustificationId = widgetDTO.ColourJustificationId,
            StatusId = widgetDTO.StatusId,
            CostPrice = widgetDTO.CostPrice,
            RetailPrice = widgetDTO.RetailPrice,
            StockLevel = widgetDTO.StockLevel,
            WidgetImage = widgetDTO.WidgetImage,
        };

        try
        {
            _context.Widgets.Add(widget);
            await _context.SaveChangesAsync();
            widgetDTO.WidgetId = widget.WidgetId;
            return new CreatedResult($"/api/widgets/{widget.WidgetId}", widgetDTO);
        }
        catch (Exception ex)
        {
            return new BadRequestObjectResult($"An error occurred while creating the Widget: {ex.Message}");
        }
    }

    public async Task<WidgetDTO>? GetWidgetAsync(int id)
    {
        var widget = await _context.Widgets
            .Include(w => w.Manufacturer)
            .Include(w => w.Colour)
            .Include(w => w.ColourJustification)
            .Include(w => w.Status)
            .AsNoTracking()
            .FirstOrDefaultAsync(w => w.WidgetId == id);
        if (widget == null)
        {
            return null;
        }
        var widgetDTO = new WidgetDTO
        {
            WidgetId = widget.WidgetId,
            Name = widget.Name,
            ManufacturerId = widget.ManufacturerId,
            ManufacturerName = widget.Manufacturer.Name,
            ColourId = widget.Colour?.ColourId,
            ColourName = widget.Colour?.Name ?? "None",
            ColourJustificationId = widget.ColourJustification?.ColourJustificationId,
            ColourJustificationText = widget.ColourJustification?.Justification ?? "None",
            StatusId = widget.StatusId,
            StatusName = widget.Status.StatusName,
            CostPrice = widget.CostPrice,
            RetailPrice = widget.RetailPrice,
            StockLevel = widget.StockLevel,
            WidgetImage = widget.WidgetImage,
        };
        return widgetDTO;
    }

    public async Task<List<WidgetDTO>> GetWidgetsAsync()
    {
        var widgets = await _context.Widgets
            .Include(w => w.Manufacturer)
            .Include(w => w.Status)
            .OrderBy(w => w.Name)
            .AsNoTracking()
            .ToListAsync();

        var widgetDTOs = new List<WidgetDTO>();

        foreach (var widget in widgets)
        {
            widgetDTOs.Add(new WidgetDTO
            {
                WidgetId = widget.WidgetId,
                Name = widget.Name,
                ManufacturerName = widget.Manufacturer.Name,
                StatusId = widget.StatusId,
                StatusName = widget.Status.StatusName,
            });
        }

        return widgetDTOs;
    }

    public async Task<List<WidgetDTO>> GetWidgetsForColourAsync(int colourId)
    {
        var widgets = await _context.Widgets
            .Include(w => w.Colour)
            .AsNoTracking()
            .Where(w => w.ColourId == colourId)
            .OrderBy(w => w.Name)
            .ToListAsync();
        var widgetDTOs = new List<WidgetDTO>();
        foreach (var widget in widgets)
        {
            widgetDTOs.Add(new WidgetDTO
            {
                WidgetId = widget.WidgetId,
                Name = widget.Name,
            });
        }
        return widgetDTOs;
    }

    public async Task<List<WidgetDTO>> GetWidgetsForColourJustificationAsync(int colourJustificationId)
    {
        var widgets = await _context.Widgets
            .Include(w => w.Colour)
            .AsNoTracking()
            .Where(w => w.ColourJustificationId == colourJustificationId)
            .OrderBy(w => w.Name)
            .ToListAsync();
        var widgetDTOs = new List<WidgetDTO>();
        foreach (var widget in widgets)
        {
            widgetDTOs.Add(new WidgetDTO
            {
                WidgetId = widget.WidgetId,
                Name = widget.Name,
            });
        }
        return widgetDTOs;
    }

    public async Task<List<WidgetDTO>> GetWidgetsForManufacturerAsync(int manufacturerId)
    {
        var widgets = await _context.Widgets
            .Include(w => w.Manufacturer)
            .AsNoTracking()
            .Where(w => w.ManufacturerId == manufacturerId)
            .OrderBy(w => w.Name)
            .ToListAsync();
        var widgetDTOs = new List<WidgetDTO>();
        foreach (var widget in widgets)
        {
            widgetDTOs.Add(new WidgetDTO
            {
                WidgetId = widget.WidgetId,
                Name = widget.Name,
            });
        }

        return widgetDTOs;
    }

    public async Task<ActionResult> UpdateWidgetAsync(int id, WidgetDTO widgetDTO)
    {
        var widgetToUpdate = _context.Widgets
            .FirstOrDefault(w => w.WidgetId == id);
        if (widgetToUpdate is null)
        {
            return new NotFoundObjectResult("No Widget with this id could be found");
        }

        widgetToUpdate.Name = widgetDTO.Name;
        widgetToUpdate.ManufacturerId = widgetDTO.ManufacturerId;
        widgetToUpdate.ColourId = widgetDTO.ColourId;
        widgetToUpdate.ColourJustificationId = widgetDTO.ColourJustificationId;
        widgetToUpdate.StatusId = widgetDTO.StatusId;
        widgetToUpdate.CostPrice = widgetDTO.CostPrice;
        widgetToUpdate.RetailPrice = widgetDTO.RetailPrice;
        widgetToUpdate.StockLevel = widgetDTO.StockLevel;
        widgetToUpdate.WidgetImage = widgetDTO.WidgetImage;

        try
        {
            await _context.SaveChangesAsync();
            return new NoContentResult();
        }
        catch (Exception ex)
        {
            return new BadRequestObjectResult($"An error occurred while updating the Widget: {ex.Message}");
        }
    }
}
