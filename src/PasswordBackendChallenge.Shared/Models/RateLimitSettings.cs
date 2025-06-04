namespace PasswordBackendChallenge.Shared.Models;

public record RateLimitSettings
{
    public bool Enabled { get; init; }
    public int PermitLimit { get; init; } = 1000;
    public int WindowInSeconds { get; init; } = 60;
}