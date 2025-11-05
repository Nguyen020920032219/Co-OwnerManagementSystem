using System.Net.Http.Json;
using Microsoft.Extensions.Caching.Memory;

namespace Co_OwnerManagementSystem.SharedLibrary.Http;

public interface IServiceTokenProvider
{
    Task<string> GetAsync(string audience, CancellationToken ct);
}

public sealed class ServiceTokenProvider : IServiceTokenProvider
{
    private readonly IMemoryCache _cache;
    private readonly IConfiguration _cfg;
    private readonly IHttpClientFactory _factory;
    private readonly ILogger<ServiceTokenProvider> _log;

    public ServiceTokenProvider(IHttpClientFactory factory, IMemoryCache cache, IConfiguration cfg,
        ILogger<ServiceTokenProvider> log)
    {
        _factory = factory;
        _cache = cache;
        _cfg = cfg;
        _log = log;
    }

    public async Task<string> GetAsync(string audience, CancellationToken ct)
    {
        if (_cfg.GetValue<bool>("Dev:DisableAuth") ||
            string.IsNullOrWhiteSpace(_cfg["Auth:TokenEndpoint"]))
        {
            _log.LogDebug("ServiceTokenProvider bypass (Dev:DisableAuth or missing TokenEndpoint).");
            return string.Empty;
        }

        var cacheKey = $"svc-token:{audience}";
        if (_cache.TryGetValue(cacheKey, out string token)) return token;

        var http = _factory.CreateClient("auth-token");
        var body = new
        {
            clientId = _cfg["Auth:ClientId"],
            clientSecret = _cfg["Auth:ClientSecret"],
            audience
        };

        using var resp = await http.PostAsJsonAsync("/oauth/token", body, ct);
        resp.EnsureSuccessStatusCode();

        var dto = await resp.Content.ReadFromJsonAsync<TokenResp>(ct)
                  ?? throw new InvalidOperationException("Token response null");
        token = dto.accessToken;

        _cache.Set(cacheKey, token, TimeSpan.FromSeconds((int)(dto.expiresIn * 0.8)));
        return token;
    }

    private sealed record TokenResp(string accessToken, string tokenType, int expiresIn);
}