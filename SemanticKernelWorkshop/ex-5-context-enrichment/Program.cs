using Microsoft.SemanticKernel;
using Kernel = Microsoft.SemanticKernel.Kernel;

var (deploymentName, endpoint, apiKey) = LlmService.LlmService.LoadSettings();
Kernel kernel = Kernel.CreateBuilder()
    .AddAzureOpenAIChatCompletion(
        deploymentName: deploymentName,
        endpoint: endpoint,
        apiKey: apiKey)
    .Build();



