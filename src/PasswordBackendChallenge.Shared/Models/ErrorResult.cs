namespace PasswordBackendChallenge.Shared.Models;

public record ErrorResult(string Message) : Result(false, -1, Message);