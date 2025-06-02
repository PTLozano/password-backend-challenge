namespace PasswordBackendChallenge.Application.ApiKey;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class ApiKeyAttribute() : ServiceFilterAttribute(typeof(ApiKeyAuthorizationFilter));

public class ApiKeyAuthorizationFilter(IApiKeyValidator apiKeyValidator) : IAuthorizationFilter
{
    private const string ApiKeyHeaderName = "x-api-key";

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        StringValues apiKey = context.HttpContext.Request.Headers[ApiKeyHeaderName];

        if (!apiKeyValidator.IsValid(apiKey))
        {
            context.Result = new UnauthorizedResult();
        }
    }
}