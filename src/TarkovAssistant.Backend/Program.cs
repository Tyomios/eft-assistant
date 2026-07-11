using Microsoft.Extensions.Http.Resilience;
using Polly;
using TarkovAssistant.Backend.Configuration;
using TarkovAssistant.Backend.Features.TarkovData;
using TarkovAssistant.Backend.Infrastructure;
using TarkovAssistant.Backend.Integrations.TarkovDev;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddSimpleConsole(options =>
{
    options.SingleLine = true;
    options.TimestampFormat = "yyyy-MM-dd HH:mm:ss.fff zzz ";
});

builder.Services
    .AddOptions<TarkovDevOptions>()
    .BindConfiguration(TarkovDevOptions.SectionName)
    .ValidateDataAnnotations()
    .Validate(options => options.BaseUrl.IsAbsoluteUri, "TarkovDev:BaseUrl must be an absolute URI.")
    .ValidateOnStart();

builder.Services.AddHttpClient<ITarkovDevClient, TarkovDevClient>((services, client) =>
    {
        var options = services.GetRequiredService<Microsoft.Extensions.Options.IOptions<TarkovDevOptions>>().Value;
        client.BaseAddress = options.BaseUrl;
        client.DefaultRequestHeaders.UserAgent.ParseAdd(options.UserAgent);
        client.DefaultRequestHeaders.Accept.ParseAdd("application/json");
    })
    .AddStandardResilienceHandler(options =>
    {
        options.TotalRequestTimeout.Timeout = TimeSpan.FromSeconds(30);
        options.AttemptTimeout.Timeout = TimeSpan.FromSeconds(10);
        options.Retry.MaxRetryAttempts = 3;
        options.Retry.Delay = TimeSpan.FromMilliseconds(500);
        options.Retry.BackoffType = DelayBackoffType.Exponential;
        options.Retry.UseJitter = true;
        options.CircuitBreaker.FailureRatio = 0.5;
        options.CircuitBreaker.MinimumThroughput = 5;
        options.CircuitBreaker.SamplingDuration = TimeSpan.FromSeconds(30);
        options.CircuitBreaker.BreakDuration = TimeSpan.FromSeconds(30);
    });

builder.Services.AddExceptionHandler<ApiExceptionHandler>();
builder.Services.AddProblemDetails();
builder.Services.AddHealthChecks();
builder.Services.AddOpenApi();

var app = builder.Build();

app.UseMiddleware<EndpointLoggingMiddleware>();
app.UseExceptionHandler();

app.MapOpenApi();
app.MapHealthChecks("/health");
app.MapTarkovDataEndpoints();

app.Run();
