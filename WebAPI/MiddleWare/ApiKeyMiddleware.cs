namespace WebAPI.MiddleWare;

public class ApiKeyMiddleware
{
    private readonly RequestDelegate _next;
    private const string APIKEYNAME = "X-Api-Key";
    private readonly string _apiKey;

    public ApiKeyMiddleware(RequestDelegate next, IConfiguration configuration)
    {
        _next = next;
        _apiKey = configuration["ApiKey"] ?? throw new ArgumentNullException("ApiKey is not configured in appsettings.json");
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var thing = context.Request.Headers.ContainsKey(APIKEYNAME);
        if (!context.Request.Headers.TryGetValue(APIKEYNAME, out var extractedApiKey) ||
            _apiKey != extractedApiKey)
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized; // Unauthorized
            await context.Response.WriteAsync("Unauthorized access. Invalid API Key.");
            return;
        }

        await _next(context); // Call the next middleware in the pipeline
    }
}
