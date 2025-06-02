using System.Text.Json.Serialization;

namespace PasswordBackendChallenge.Shared.Dtos;

public record PasswordResponse(bool IsValid)
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string Message { get; init; }
}