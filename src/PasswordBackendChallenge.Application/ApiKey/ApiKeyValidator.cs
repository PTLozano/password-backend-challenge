namespace PasswordBackendChallenge.Application.ApiKey;

public class ApiKeyValidator(IConfiguration configuration) : IApiKeyValidator
{
    private const string ApiKeyName = "x-api-key";

    public bool IsValid(string apiKey)
    {
        string validApiKey = configuration.GetValue<string>(ApiKeyName);

        if (string.IsNullOrWhiteSpace(validApiKey))
        {
            return true;
        }

        return apiKey == validApiKey;
    }
}

public interface IApiKeyValidator
{
    bool IsValid(string apiKey);
}