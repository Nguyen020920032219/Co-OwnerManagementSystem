using Microsoft.AspNetCore.Http;

namespace Co_OwnerManagementSystem.SharedLibrary.Observability;

public sealed class CorrelationIdMiddleware
{
    public const string HeaderName = "X-Correlation-Id";
    private readonly ILogger<CorrelationIdMiddleware> _logger;
    private readonly RequestDelegate _next;

    public CorrelationIdMiddleware(RequestDelegate next, ILogger<CorrelationIdMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext ctx)
    {
        var cid = ctx.Request.Headers.TryGetValue(HeaderName, out var v) && !string.IsNullOrWhiteSpace(v)
            ? v.ToString()
            : Guid.NewGuid().ToString();

        ctx.TraceIdentifier = cid;
        ctx.Response.OnStarting(() =>
        {
            if (!ctx.Response.Headers.ContainsKey(HeaderName))
                ctx.Response.Headers.Add(HeaderName, cid);
            return Task.CompletedTask;
        });

        using (_logger.BeginScope(new Dictionary<string, object?> { ["CorrelationId"] = cid }))
        {
            await _next(ctx);
        }
    }
}

public static class CorrelationIdAppExtensions
{
    public static IApplicationBuilder UseCorrelationId(this IApplicationBuilder app)
    {
        return app.UseMiddleware<CorrelationIdMiddleware>();
    }
}