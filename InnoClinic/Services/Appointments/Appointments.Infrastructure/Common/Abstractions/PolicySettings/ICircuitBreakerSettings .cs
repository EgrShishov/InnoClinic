public interface ICircuitBreakerSettings
{
    public int FailureThreshold { get; }
    public TimeSpan OpenCircuitDuration { get; }
    public TimeSpan SamplingDuration { get; }
    public int MinimumThroughput { get; }
    public Action<DelegateResult<HttpResponseMessage>, TimeSpan> OnBreak { get; }
    public TimeSpan DurationOfBreak { get; }
    public Action OnReset { get; }
    public Action OnHalfOpen { get; }
}