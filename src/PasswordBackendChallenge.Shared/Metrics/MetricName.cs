namespace PasswordBackendChallenge.Shared.Metrics;

public abstract class MetricName
{
    public static readonly Tuple<string, string> ProcessDuration = new("password_process_duration", "Process duration");
    public static readonly Tuple<string, string> ErrorType = new("password_error_type", "Count of error by type");
    public static readonly Tuple<string, string> Result = new("password_result", "Number of result by type");
}