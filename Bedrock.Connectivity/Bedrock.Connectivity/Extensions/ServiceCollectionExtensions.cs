using Amazon.Bedrock;
using Amazon.BedrockRuntime;
using Amazon.Extensions.NETCore.Setup;
using Bedrock.Connectivity.Model;
using Bedrock.Connectivity.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Bedrock.Connectivity.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddBedrockServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Register settings from appsettings.json
            services.Configure<BedrockSettings>(configuration.GetSection("BedrockSettings"));

            // Register Amazon Bedrock Runtime + Bedrock clients (use IRSA / environment / default chain)
            services.AddAWSService<IAmazonBedrockRuntime>();
            services.AddAWSService<IAmazonBedrock>();

            // Register custom services
            services.AddSingleton<IBedrockModelSelector, BedrockModelSelector>();
            services.AddSingleton<BedrockService>();

            return services;
        }
    }
}
