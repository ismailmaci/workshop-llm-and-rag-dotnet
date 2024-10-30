using Microsoft.SemanticKernel;
using Kernel = Microsoft.SemanticKernel.Kernel;

var (deploymentName, endpoint, apiKey) = LlmService.LlmService.LoadSettings();

Kernel kernel = Kernel.CreateBuilder()
    .AddAzureOpenAIChatCompletion(
        deploymentName: deploymentName,
        endpoint: endpoint,
        apiKey: apiKey)
    .Build();

Console.Write("You: ");
var userInput = Console.ReadLine() ?? string.Empty;
await foreach (var update in kernel.InvokePromptStreamingAsync(userInput))
{
    await Task.Delay(50);
    Console.Write(update);
}
