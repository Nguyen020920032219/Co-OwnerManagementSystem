using Co_OwnerManagementSystem.SharedLibrary.Observability;

namespace Co_OwnerManagementSystem.SharedLibrary.Http;

public static class HttpClientResilienceExtensions
{
    public static IHttpClientBuilder WithStandardResilience(this IHttpClientBuilder b, TimeSpan? timeout = null)
    {
        b.AddHttpMessageHandler<CorrelationIdHandler>();
        b.AddStandardResilienceHandler(opt =>
        {
            opt.TotalRequestTimeout.Timeout = timeout ?? TimeSpan.FromSeconds(15);
            opt.Retry.MaxRetryAttempts = 3;
            opt.CircuitBreaker.SamplingDuration = TimeSpan.FromSeconds(20);
            opt.CircuitBreaker.FailureRatio = 0.5;
        });
        return b;
    }

    public static IServiceCollection AddSharedHttpInfra(this IServiceCollection s)
    {
        return s.AddHttpContextAccessor()
            .AddTransient<CorrelationIdHandler>();
    }
}