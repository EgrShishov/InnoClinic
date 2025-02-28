public interface IRetrySettings
{
    public int RetryCount { get; }
    public TimeSpan InitialRetryValue { get; }
    public Func<int, TimeSpan> SleepDurationProvider { get; }
    public Action<DelegateResult<HttpResponseMessage>, TimeSpan, int, Context> OnRetry { get; }
}