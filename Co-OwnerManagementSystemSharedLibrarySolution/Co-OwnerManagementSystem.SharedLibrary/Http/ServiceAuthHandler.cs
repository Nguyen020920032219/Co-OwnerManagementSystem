using System.Net.Http.Headers;

namespace Co_OwnerManagementSystem.SharedLibrary.Http;

public sealed class ServiceAuthHandler : DelegatingHandler
{
    private readonly string _audience;
    private readonly IServiceTokenProvider _tokens;

    public ServiceAuthHandler(IServiceTokenProvider tokens, string audience)
    {
        _tokens = tokens;
        _audience = audience;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken ct)
    {
        var token = await _tokens.GetAsync(_audience, ct);
        if (!string.IsNullOrWhiteSpace(token))
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        return await base.SendAsync(request, ct);
    }
}

public static class ServiceAuthHandlerExtensions
{
    public static IHttpClientBuilder AddServiceAuth(this IHttpClientBuilder b, string audience)
    {
        return b.AddHttpMessageHandler(sp =>
            new ServiceAuthHandler(sp.GetRequiredService<IServiceTokenProvider>(), audience));
    }
}