
public class CircuitBreakerSettings : ICircuitBreakerSettings
{
    public int FailureThreshold { get; set; }

    public TimeSpan OpenCircuitDuration {  get; set; }

    public TimeSpan SamplingDuration {  get; set; }

    public int MinimumThroughput { get; set; }

    public Action<DelegateResult<HttpResponseMessage>, TimeSpan> OnBreak { get; set; }

    public TimeSpan DurationOfBreak { get; set; }

    public Action OnReset {  get; set; }

    public Action OnHalfOpen { get; set; }
}
