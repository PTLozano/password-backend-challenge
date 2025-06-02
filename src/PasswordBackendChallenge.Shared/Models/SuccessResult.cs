namespace PasswordBackendChallenge.Shared.Models;

public record SuccessResult(int ValidCharacterCount) : Result(true, ValidCharacterCount, string.Empty);