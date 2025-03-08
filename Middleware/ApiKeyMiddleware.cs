namespace aaura.api.Middleware;

public class ApiKeyMiddleware(RequestDelegate next)
{
    private readonly RequestDelegate _next = next;
    private readonly string apiHeaderKey = "x-api-key";
    public async Task InvokeAsync(HttpContext context)
    {
        if (!context.Request.Headers.TryGetValue(apiHeaderKey, out var providedKey) || providedKey != "abc123")
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync("Didnt work :(");
            return;
        }
        await _next(context);
    }
}