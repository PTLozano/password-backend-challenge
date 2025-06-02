namespace PasswordBackendChallenge.Tests;

public static class Helper
{
    internal static PasswordSettings CreatePasswordSettings =>
        new()
        {
            MinimumPasswordLength = 9,
            MaximumPasswordLength = 20,
            Complexity =
            [
                new(
                    true,
                    1,
                    10,
                    1,
                    "letras maiúsculas",
                    "ABCDEFGHIJKLMNOPQRSTUVWXYZ"
                ),
                new(
                    true,
                    1,
                    10,
                    1,
                    "letras minúsculas",
                    "abcdefghijklmnopqrstuvwxyz"
                ),
                new(
                    true,
                    1,
                    10,
                    1,
                    "números",
                    "0123456789"
                ),
                new(
                    true,
                    1,
                    10,
                    1,
                    "caracteres especiais",
                    "!@#$%^&*()-+"
                )
            ]
        };
}
public class TestOptionsMonitor<TOptions>(TOptions currentValue) : IOptionsMonitor<TOptions>
{
    private Action<TOptions, string> _listener;

    public TOptions CurrentValue { get; private set; } = currentValue;

    public TOptions Get(string name)
    {
        return CurrentValue;
    }

    public void Set(TOptions value)
    {
        CurrentValue = value;
        _listener.Invoke(value, null);
    }

    public IDisposable OnChange(Action<TOptions, string> listener)
    {
        _listener = listener;
        return Mock.Of<IDisposable>();
    }
}