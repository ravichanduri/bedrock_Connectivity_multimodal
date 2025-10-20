# MultiModelSelector

A **.NET console / microservices project** demonstrating **multimodal and multi-model support** for Amazon Bedrock.  
Implements **primary/secondary model fallback**, **per-region caching**, and is compatible with **IRSA or AWS SDK default credentials**.

---

## Features

- **Primary & Secondary Fallback:** Selects primary model for each input type; if unavailable, automatically falls back to secondary. Throws error if neither is available.
- **Multimodal Support:** Supports different model types such as `text`, `image`, and `reasoning`.
- **Region-Aware Caching:** Thread-safe cache per AWS region to reduce API calls.
- **AWS SDK Integration:** Uses `IAmazonBedrock` and `IAmazonBedrockRuntime`. Works with IRSA, environment variables, or default AWS credential chain.
- **Dependency Injection:** Clean DI setup for ASP.NET Core or Console applications.

---

## Folder Structure

```
Bedrock.Connectivity/
│
├── Program.cs
├── appSettings.json
│
├── Extensions/
│   └── ServiceCollectionExtensions.cs
│
├── Model/
│   └── BedrockSettings.cs
│
└── Services/
    ├── IBedrockModelSelector.cs
    ├── BedrockModelSelector.cs
    └── BedrockService.cs
```

---

## Configuration

### `appSettings.json`

```json
{
  "BedrockSettings": {
    "Models": {
      "text": {
        "Primary": "<Primary Text Model ID>",
        "Secondary": "<Secondary Text Model ID>"
      },
      "image": {
        "Primary": "<Primary Image Model ID>",
        "Secondary": "<Secondary Image Model ID>"
      },
      "reasoning": {
        "Primary": "<Primary Reasoning Model ID>",
        "Secondary": "<Secondary Reasoning Model ID>"
      }
    }
  }
}
```
Replace `<Primary ...>` and `<Secondary ...>` with actual Amazon Bedrock model IDs.  
Leaving empty will throw an exception if no suitable model is found.

---

## Setup & Installation

Clone the repository:
```shell
git clone <repository-url>
cd Bedrock.Connectivity
```

Install required NuGet packages:
```shell
dotnet add package AWSSDK.Extensions.NETCore.Setup
dotnet add package AWSSDK.Bedrock
dotnet add package AWSSDK.BedrockRuntime
```

Configure AWS credentials:
- **IRSA:** Use IAM Role for Service Account (Kubernetes).
- **Local development:** Use environment variables or default AWS profile.

---

## Usage

Start the application:
```shell
dotnet run
```

The program will:
- Fetch available foundation models for the current AWS region (cached).
- Select primary or secondary model for the input type (text, image, reasoning).
- Invoke the model using `IAmazonBedrockRuntime`.
- Print the result to the console.

---

## Example

```csharp
var response = await bedrockService.ProcessAsync("text", "Hello from .NET Bedrock integration!");
Console.WriteLine($"Response: {response}");
```
- Primary model used if available.
- Fallback to secondary if primary missing.
- Exception thrown if neither is found.

---

## Architecture Overview
- **BedrockService:** Wraps Bedrock runtime calls and orchestrates model selection.
- **BedrockModelSelector:** Handles primary/secondary selection and caching.
- **ServiceCollectionExtensions:** Registers AWS clients and services for DI.
- **Model:** Model preferences loaded from `appSettings.json`.

---

## Benefits
- Reduces runtime failures from unavailable models.
- Supports multi-region deployments.
- Easily extensible for new model types or regions.
- Fully compatible with .NET DI and AWS best practices.
