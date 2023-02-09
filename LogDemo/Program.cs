using DemoDependency;
using Serilog;
using ILogger = Serilog.ILogger;

var builder = WebApplication.CreateBuilder(args);

// Serilog setup
builder.Logging.ClearProviders();
builder.Host.UseSerilog((_, logConfig) => logConfig.ReadFrom.Configuration(builder.Configuration));

// Register the "dependency" that uses MS ILogger<T>
builder.Services.AddScoped<ClassFromDependency>();

var app = builder.Build();

// More Serilog setup
app.UseSerilogRequestLogging();

// API setup
app.MapGet(
    "/",
    (ILogger serilogLogger, ClassFromDependency dependency) => // NOTE: This is Serilog's ILogger
    {
        serilogLogger.Information("Hello from '/'");

        dependency.SayHello();

        return Task.FromResult(Results.Ok());
    });

app.Run();