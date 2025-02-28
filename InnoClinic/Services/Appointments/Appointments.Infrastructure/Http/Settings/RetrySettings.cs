public class RetrySettings : IRetrySettings
{
    public static string SectionName = "RetrySettings";
    public int RetryCount { get; set; }
    public Func<int, TimeSpan> SleepDurationProvider => 
        retryAttempt
            => TimeSpan.FromMilliseconds(InitialRetryValue.TotalMilliseconds * Math.Pow(2, retryAttempt - 1));
    public Action<DelegateResult<HttpResponseMessage>, TimeSpan, int, Context> OnRetry { get; set; }
    public TimeSpan InitialRetryValue { get; set; }
}