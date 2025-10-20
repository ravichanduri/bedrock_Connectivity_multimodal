using Amazon;
using Amazon.BedrockRuntime;
using Amazon.BedrockRuntime.Model;


namespace Bedrock.Connectivity.Services
{
    public class BedrockService
    {
        private readonly IAmazonBedrockRuntime _runtimeClient;
        private readonly IBedrockModelSelector _modelSelector;

        public BedrockService(IAmazonBedrockRuntime runtimeClient, IBedrockModelSelector modelSelector)
        {
            _runtimeClient = runtimeClient;
            _modelSelector = modelSelector;
        }

        public async Task<string> ProcessAsync(string inputType, string inputData)
        {
            var modelId = await _modelSelector.GetModelForTypeAsync(inputType);

            var request = new InvokeModelRequest
            {
                ModelId = modelId,
                Body = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(inputData))
            };

            var response = await _runtimeClient.InvokeModelAsync(request);

            using var reader = new StreamReader(response.Body);
            return await reader.ReadToEndAsync();
        }
    }
}
