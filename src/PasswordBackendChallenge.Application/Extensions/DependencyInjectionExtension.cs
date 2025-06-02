namespace PasswordBackendChallenge.Application.Extensions;

public static class DependencyInjectionExtension
{
    public static void AddServices(this IServiceCollection services)
    {
        services.AddHealthChecks();
        services.AddScoped<IValidator<PasswordRequest>, PasswordRequestValidator>();
        services.AddScoped<IPasswordService, PasswordService>();
        services.AddScoped<ApiKeyAuthorizationFilter>();
        services.AddTransient<IApiKeyValidator, ApiKeyValidator>();
        services.AddSwaggerGen();
    }

    public static void AddRateLimiting(this IServiceCollection services)
    {
        services.AddRateLimiter(options =>
        {
            options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
            options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
            {
                string path = httpContext.Request.Path.ToString();

                // Different limits for different paths
                if (path.StartsWith("/api/password/validate", StringComparison.InvariantCultureIgnoreCase))
                {
                    return RateLimitPartition.GetFixedWindowLimiter(
                        partitionKey: $"{httpContext.Connection.RemoteIpAddress}-public",
                        factory: _ => new()
                        {
                            PermitLimit = 5,
                            Window = TimeSpan.FromMinutes(10)
                        });
                }

                return RateLimitPartition.GetFixedWindowLimiter(
                    partitionKey: httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown",
                    factory: _ => new()
                    {
                        PermitLimit = 1000,
                        Window = TimeSpan.FromSeconds(1)
                    });
            });
        });
    }
}