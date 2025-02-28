using Microsoft.Extensions.DependencyInjection;
using Polly.Extensions.Http;
using Polly.Timeout;
using System.Net;


public static class HttpClientBuilderExtensions
{
    public static IHttpClientBuilder AddRetryPolicy(
        this IHttpClientBuilder builder,
        IRetrySettings settings)
    {
        return builder
            .AddPolicyHandler(HttpPolicyExtensions
                .HandleTransientHttpError()
                .Or<TimeoutRejectedException>()
                .WaitAndRetryAsync(
                    settings.RetryCount,
                    settings.SleepDurationProvider,
                    settings.OnRetry));
    }

    public static IHttpClientBuilder AddCircuitBreakerPolicy(
        this IHttpClientBuilder builder,
        ICircuitBreakerSettings settings)
    {
        return builder
            .AddPolicyHandler(HttpPolicyExtensions
                .HandleTransientHttpError()
                .Or<TimeoutRejectedException>()
                .OrResult(r => r.StatusCode == HttpStatusCode.TooManyRequests)
                .AdvancedCircuitBreakerAsync(
                    settings.FailureThreshold,
                    settings.SamplingDuration,
                    settings.MinimumThroughput,
                    settings.DurationOfBreak,
                    settings.OnBreak,
                    settings.OnReset,
                    settings.OnHalfOpen));
    }

    private static IHttpClientBuilder AddTimeoutPolicy(
        this IHttpClientBuilder httpClientBuilder,
        TimeSpan timeout)
    {
        return httpClientBuilder.AddPolicyHandler(
            Policy.TimeoutAsync<HttpResponseMessage>(timeout));
    }
}