namespace CAMS.Common.Settings;

public class ServiceBusSettings
{
    public string ConnectionString { get; set; } = null!;
    public int MaximumRetries { get; set; } = 5;
    public double RetryDelaySeconds { get; set; } = 0.8;
    public double MaximumRetryDelaySeconds { get; set; } = 30;
}
