namespace PasswordBackendChallenge.Application.Extensions;

public static class TelemetryExtension
{
    public static void AddPasswordMetrics(this IServiceCollection services)
    {
        services
           .AddSingleton<IPasswordMetric, PasswordMetric>()
           .AddPasswordTelemetry();
    }

    private static IServiceCollection AddPasswordTelemetry(this IServiceCollection services)
    {
        const string serviceName = "PasswordBackendChallenge";

        IList<string> unwantedRoute = ["/healthy", "/metrics", "/swagger"];

        services
           .AddMetrics()
           .AddOpenTelemetry()
           .ConfigureResource(resource => resource.AddService(serviceName))
           .WithTracing(tracing =>
            {
                tracing
                   .ConfigureResource(resource => resource.AddService(serviceName))
                   .AddSource("PasswordBackendChallenge.*")
                   .AddAspNetCoreInstrumentation(x =>
                    {
                        x.RecordException = true;
                        x.Filter = ctx =>
                            ctx.Request.Path.HasValue && !unwantedRoute.Any(ctx.Request.Path.Value.StartsWith);
                    })
                   .AddHttpClientInstrumentation(x =>
                    {
                        x.RecordException = true;
                        x.EnrichWithHttpRequestMessage = (activity, message) => { activity.DisplayName = $"{message.Method} {message.RequestUri}"; };
                    });

                tracing.AddOtlpExporter();
            })
           .WithMetrics(metrics =>
            {
                metrics
                   .AddMeter(
                        serviceName,
                        "Microsoft.AspNetCore.Hosting",
                        "Microsoft.AspNetCore.Server.Kestrel",
                        "System.Net.Http",
                        "PasswordBackendChallenge.*"
                    )
                   .AddRuntimeInstrumentation()
                   .AddAspNetCoreInstrumentation()
                   .AddHttpClientInstrumentation()
                   .AddProcessInstrumentation()
                   .AddPrometheusHttpListener(opt => opt.UriPrefixes = new[] { "http://*:9096/" })
                   .AddPrometheusExporter()
                   .AddView(
                        instrumentName: "password_process_duration",
                        new ExplicitBucketHistogramConfiguration
                        {
                            Boundaries = [0.01, 0.02, 0.05, 0.1, 0.25, 0.5, 1, 1.5, 2, 5, 10, 30]
                        }
                    )
                   .AddView("http.server.request.duration",
                            new ExplicitBucketHistogramConfiguration
                            {
                                Boundaries = [0, 0.005, 0.01, 0.025, 0.05, 0.075, 0.1, 0.25, 0.5, 0.75, 1, 2.5, 5, 7.5, 10]
                            });

                metrics.AddOtlpExporter((_, readerOptions) =>
                                            readerOptions.PeriodicExportingMetricReaderOptions.ExportIntervalMilliseconds = 10000);
            });

        return services;
    }
}