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
app.UseSerilogRequestLogging(
    opt =>
    {
        opt.IncludeQueryInRequestPath = false; // default
        opt.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
        {
            // Example of what you can add to request logs
            diagnosticContext.Set("QueryString", httpContext.Request.QueryString);
        };
    });

// API setup
app.MapGet(
    "/",
    (HttpContext context, ILogger serilogLogger, ClassFromDependency dependency) => // NOTE: This is Serilog's ILogger
    {
        serilogLogger.Information("Hello from '/'");

        dependency.SayHello();

        return Task.FromResult(Results.Ok());
    });

app.Run();