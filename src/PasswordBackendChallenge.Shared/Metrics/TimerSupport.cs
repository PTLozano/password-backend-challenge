namespace PasswordBackendChallenge.Shared.Metrics;

public delegate void TimerSupportHandler(double e);

public sealed class TimerSupport : IDisposable
{
    private readonly DateTime _start = DateTime.UtcNow;

    public void Dispose()
    {
        TimeSpan timer = DateTime.UtcNow - _start;
        TimerSupportHandler?.Invoke(timer.TotalSeconds);
    }

    public event TimerSupportHandler TimerSupportHandler;
}