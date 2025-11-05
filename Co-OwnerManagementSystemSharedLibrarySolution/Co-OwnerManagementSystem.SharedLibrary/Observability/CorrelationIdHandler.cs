using Microsoft.AspNetCore.Http;

namespace Co_OwnerManagementSystem.SharedLibrary.Observability;

public sealed class CorrelationIdHandler : DelegatingHandler
{
    private readonly IHttpContextAccessor _http;

    public CorrelationIdHandler(IHttpContextAccessor http)
    {
        _http = http;
    }

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken ct)
    {
        var ctx = _http.HttpContext;
        var cid = ctx?.TraceIdentifier ?? Guid.NewGuid().ToString();
        if (!request.Headers.Contains(CorrelationIdMiddleware.HeaderName))
            request.Headers.TryAddWithoutValidation(CorrelationIdMiddleware.HeaderName, cid);
        return base.SendAsync(request, ct);
    }
}