using System.ComponentModel.DataAnnotations;

namespace WebAPI.DTOs
{
    public class WidgetDTO
    {
        public int WidgetId { get; set; }

        public string Name { get; set; } = default!;

        public int ManufacturerId { get; set; }

        public string? ManufacturerName { get; set; }

        public int? ColourId { get; set; }

        public string? ColourName { get; set; }

        public int? ColourJustificationId { get; set; }

        public string? ColourJustificationText { get; set; }

        public int StatusId { get; set; }

        public string? StatusName { get; set; }

        public decimal CostPrice { get; set; }

        public decimal RetailPrice { get; set; }

        public int StockLevel { get; set; }

        public byte[]? WidgetImage { get; set; }
    }
}
