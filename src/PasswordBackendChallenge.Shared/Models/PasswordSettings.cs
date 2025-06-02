namespace PasswordBackendChallenge.Shared.Models;

public record PasswordSettings
{
    public int MinimumPasswordLength { get; init; } = 9;
    public int MaximumPasswordLength { get; init; } = 64;
    public required IReadOnlyCollection<Complexity> Complexity { get; init; } = [];
}
public record RateLimitSettings
{
    public bool Enabled { get; init; }
    public int PermitLimit { get; init; } = 1000;
    public int WindowInSeconds { get; init; } = 60;
}