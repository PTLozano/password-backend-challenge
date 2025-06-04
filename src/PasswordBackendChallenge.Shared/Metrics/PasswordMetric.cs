namespace PasswordBackendChallenge.Shared.Metrics;

public sealed class PasswordMetric : IPasswordMetric
{
    private readonly Histogram<double> _processDuration;
    private readonly Counter<int> _result;
    private readonly Counter<int> _resultErrorType;

    public PasswordMetric(IMeterFactory meterFactory)
    {
        Meter meter = meterFactory.Create("Password");
        _processDuration = meter.CreateHistogram<double>(MetricName.ProcessDuration.Item1, null, MetricName.ProcessDuration.Item2);
        _resultErrorType = meter.CreateCounter<int>(MetricName.ErrorType.Item1, null, MetricName.ErrorType.Item2);
        _result = meter.CreateCounter<int>(MetricName.Result.Item1, null, MetricName.Result.Item2);
    }

    public IDisposable AddProcessDuration(string alias)
    {
        TimerSupport timer = new();
        timer.TimerSupportHandler += e => { _processDuration.Record(e, SetAlias(alias)); };

        return timer;
    }

    public void AddErrorCount(string error, int i = 1)
    {
        _resultErrorType.Add(i, SetAlias("error_" + error));
    }

    public void AddResult(string name)
    {
        _result.Add(1, SetAlias(name));
    }

    private static TagList SetAlias(string value, string key = "alias")
    {
        return new() { { key, value } };
    }
}

public interface IPasswordMetric
{
    IDisposable AddProcessDuration(string alias);
    void AddErrorCount(string error, int i = 1);
    void AddResult(string name);
}