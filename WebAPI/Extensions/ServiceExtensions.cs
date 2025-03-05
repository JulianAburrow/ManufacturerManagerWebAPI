namespace WebAPI.Extensions;

public static class ServiceExtensions
{
    public static void ConfigureSqlConnections(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ManufacturerManagerDbContext>(
            options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("ManufacturerManager")));
    }

    public static void AddDependencies(this IServiceCollection services)
    {
        services.AddTransient<IColourHandler, ColourHandler>();
        services.AddTransient<IColourJustificationHandler, ColourJustificationHandler>();
        services.AddTransient<IManufacturerStatusHandler, ManufacturerStatusHandler>();
        services.AddTransient<IWidgetStatusHandler, WidgetStatusHandler>();
    }
}
