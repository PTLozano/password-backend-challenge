namespace PasswordBackendChallenge.Shared.Models;

public record Result(
    bool Success,
    int ValidCharacterCount,
    string Message
)
{
    public bool IsValid => Success;
};