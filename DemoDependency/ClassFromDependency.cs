using Microsoft.Extensions.Logging;

namespace DemoDependency;

public record ClassFromDependency(ILogger<ClassFromDependency> Logger)
{
    public void SayHello()
    {
        Logger.LogInformation("Hello from dependency!");
    }
}