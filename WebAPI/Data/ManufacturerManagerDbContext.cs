using DataAccess.Configuration;

namespace WebAPI.Data;

public class ManufacturerManagerDbContext(DbContextOptions<ManufacturerManagerDbContext> options) : DbContext(options)
{
    public DbSet<ManufacturerModel> Manufacturers => Set<ManufacturerModel>();

    public DbSet<ManufacturerStatusModel> ManufacturerStatuses => Set<ManufacturerStatusModel>();

    public DbSet<WidgetModel> Widgets => Set<WidgetModel>();

    public DbSet<WidgetStatusModel> WidgetStatuses => Set<WidgetStatusModel>();

    public DbSet<ColourModel> Colours => Set<ColourModel>();

    public DbSet<ColourJustificationModel> ColourJustifications => Set<ColourJustificationModel>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        foreach (var property in builder.Model.GetEntityTypes()
            .SelectMany(e => e.GetProperties()
            .Where(p => p.ClrType == typeof(string))))
        {
            property.SetIsUnicode(false);
        }

        builder.ApplyConfiguration(new ColourConfiguration());
        builder.ApplyConfiguration(new ColourJustificationConfiguration());
        builder.ApplyConfiguration(new ManufacturerConfiguration());
        builder.ApplyConfiguration(new ManufacturerStatusConfiguration());
        builder.ApplyConfiguration(new WidgetConfiguration());
        builder.ApplyConfiguration(new WidgetStatusConfiguration());
    }
}
