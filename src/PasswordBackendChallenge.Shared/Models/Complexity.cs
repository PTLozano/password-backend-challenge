namespace PasswordBackendChallenge.Shared.Models;

public record Complexity(
    bool Enabled,
    int? MinimumLength,
    int? MaximumLength,
    int? MaximumRepeatCharCount,
    string Identifier,
    string Characters
);