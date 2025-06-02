WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddServices();
builder.Services.AddPasswordMetrics();
builder.Services.Configure<PasswordSettings>(builder.Configuration.GetSection(nameof(PasswordSettings)));

RateLimitSettings rateLimitSettings = new();
builder.Configuration.GetSection(nameof(RateLimitSettings)).Bind(rateLimitSettings);

if(rateLimitSettings.Enabled)
{
    builder.Services.AddRateLimiting();
}

WebApplication app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
}

if (!app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseOpenTelemetryPrometheusScrapingEndpoint("/metrics");

app.MapHealthChecks("/healthy");

if(rateLimitSettings.Enabled)
{
    app.UseRateLimiter();
}

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    "{controller}/{action=Index}/{id?}");

app.Run();