namespace PasswordBackendChallenge.Shared.Models;

public record RateLimitSettings
{
    public bool Enabled { get; init; }
    public int PasswordPermitLimit { get; init; } = 1000;
    public int PasswordWindowInSeconds { get; init; } = 60;
    public int GlobalPermitLimit { get; init; } = 1000;
    public int GlobalWindowInSeconds { get; init; } = 60;
}