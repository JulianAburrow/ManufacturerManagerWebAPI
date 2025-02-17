using System.Net;

namespace WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class WidgetController(ManufacturerManagerDbContext context) : ControllerBase
{
    private readonly ManufacturerManagerDbContext _context = context;

    [HttpGet]
    public async Task<ActionResult<List<WidgetDTO>>> GetWidgets()
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

        return Ok(widgetDTOs);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<WidgetDTO>> GetWidget(int id)
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
            return NotFound();
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
        return Ok(widgetDTO);
    }

    [HttpGet("check/{widgetName}/{id}")]
    public async Task<ActionResult<HttpStatusCode>> CheckForExistingWidget(string widgetName, int id)
    {
        var widgets = await _context.Widgets
            .Where(
                w =>
                    w.Name.Replace(" ", "") == widgetName.Replace(" ", ""))
            .ToListAsync();

        if (id > 0)
        {
            widgets = widgets.Where(w => w.WidgetId != id).ToList();
        }

        if (widgets.Count > 0)
        {
            return Conflict();
        }

        return Ok();
    }

    [HttpGet("widgetsbymanufacturer/{manufacturerId}")]
    public async Task<ActionResult<WidgetDTO>> GetWidgetsForManufacturer(int manufacturerId)
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

        return Ok(widgetDTOs);
    }

    [HttpGet("widgetsbycolour/{colourId}")]
    public async Task<ActionResult<List<WidgetDTO>>> GetWidgetsForColour(int colourId)
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
        return Ok(widgetDTOs);
    }

    [HttpGet(template: "widgetsbycolourjustification/{colourJustificationId}")]
    public async Task<ActionResult<List<WidgetDTO>>> GetWidgetsForColourJustification(int colourJustificationId)
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
        return Ok(widgetDTOs);
    }

    [HttpPost]
    public async Task<ActionResult> CreateWidget(WidgetDTO widgetDTO)
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
            return Ok();
        }
        catch
        {
            return BadRequest();
        }        
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateWidget(int id, WidgetDTO widgetDTO)
    {
        var widgetToUpdate = _context.Widgets
            .FirstOrDefault(w => w.WidgetId == id);
        if (widgetToUpdate is null)
        {
            return NotFound();
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
            return Ok(); 
        }
        catch
        {
            return BadRequest();
        }
    }
}
