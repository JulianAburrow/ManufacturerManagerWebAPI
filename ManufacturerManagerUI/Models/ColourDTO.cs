﻿namespace ManufacturerManagerUI.Models;

public class ColourDTO
{
    public int ColourId { get; set; }

    [Required(ErrorMessage = "{0} is required")]
    [StringLength(20, ErrorMessage = "{0} cannot be more that {1} characters")]
    public string Name { get; set; } = default!;

    public int WidgetCount { get; set; }
}
