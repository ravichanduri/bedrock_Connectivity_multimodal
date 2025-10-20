using Bedrock.Connectivity.Extensions;
using Bedrock.Connectivity.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

class Program
{
    static async Task Main(string[] args)
    {
        // Load configuration
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false)
            .Build();

        // Setup DI
        var services = new ServiceCollection();
        services.AddBedrockServices(configuration);

        var provider = services.BuildServiceProvider();
        var bedrockService = provider.GetRequiredService<BedrockService>();

        try
        {
            var result = await bedrockService.ProcessAsync("text", "Hello from .NET Bedrock integration!");
            Console.WriteLine($"Response: {result}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}
