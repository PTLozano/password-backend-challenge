namespace PasswordBackendChallenge.Shared.Models;

public record PasswordSettings
{
    public int MinimumPasswordLength { get; init; } = 9;
    public int MaximumPasswordLength { get; init; } = 64;
    public required IReadOnlyCollection<Complexity> Complexity { get; init; } = [];
}