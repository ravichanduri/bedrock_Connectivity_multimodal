using Amazon.Bedrock;
using Amazon.Bedrock.Model;
using Amazon.BedrockRuntime;
using Bedrock.Connectivity.Model;
using Microsoft.Extensions.Options;
using System.Collections.Concurrent;

namespace Bedrock.Connectivity.Services
{
    public class BedrockModelSelector : IBedrockModelSelector
    {
        private readonly IAmazonBedrock _bedrockClient;
        private readonly IAmazonBedrockRuntime _runtimeClient;
        private readonly BedrockSettings _settings;

        private static readonly ConcurrentDictionary<string, List<string>> _regionModelCache = new();

        public BedrockModelSelector(
            IAmazonBedrock bedrockClient,
            IAmazonBedrockRuntime runtimeClient,
            IOptions<BedrockSettings> settings)
        {
            _bedrockClient = bedrockClient;
            _runtimeClient = runtimeClient;
            _settings = settings.Value;
        }

        public async Task<string> GetModelForTypeAsync(string inputType)
        {
            var region = _runtimeClient.Config.RegionEndpoint?.SystemName ?? "unknown-region";

            // Get cached or fetch models
            var models = await GetCachedModelsForRegionAsync(region);

            if (!_settings.Models.TryGetValue(inputType, out var preference))
                throw new InvalidOperationException($"No model preference found for input type '{inputType}'.");

            // 1️⃣ Try Primary
            if (!string.IsNullOrEmpty(preference.Primary) &&
                models.Any(m => m.Contains(preference.Primary, StringComparison.OrdinalIgnoreCase)))
            {
                var match = models.First(m => m.Contains(preference.Primary, StringComparison.OrdinalIgnoreCase));
                Console.WriteLine($"✅ Using Primary Model: {match}");
                return match;
            }

            // 2️⃣ Try Secondary
            if (!string.IsNullOrEmpty(preference.Secondary) &&
                models.Any(m => m.Contains(preference.Secondary, StringComparison.OrdinalIgnoreCase)))
            {
                var match = models.First(m => m.Contains(preference.Secondary, StringComparison.OrdinalIgnoreCase));
                Console.WriteLine($"⚠️ Primary not found, using Secondary Model: {match}");
                return match;
            }

            // 3️⃣ Throw if neither found
            throw new Exception($"❌ No suitable model found for '{inputType}' in region '{region}'.");
        }

        private async Task<List<string>> GetCachedModelsForRegionAsync(string region)
        {
            if (_regionModelCache.TryGetValue(region, out var cached))
                return cached;

            Console.WriteLine($"Fetching model list from Bedrock for region: {region}");

            var response = await _bedrockClient.ListFoundationModelsAsync(new ListFoundationModelsRequest());

            var models = response.ModelSummaries
                .Select(m => m.ModelId)
                .Where(id => !string.IsNullOrEmpty(id))
                .ToList();

            _regionModelCache.TryAdd(region, models);

            return models;
        }
    }
}
