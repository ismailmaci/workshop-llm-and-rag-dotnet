using Microsoft.SemanticKernel;
using Kernel = Microsoft.SemanticKernel.Kernel;

var settings = LlmService.LlmService.LoadSettings();

Kernel kernel = Kernel.CreateBuilder()
    .AddAzureOpenAIChatCompletion(
        deploymentName: settings.deploymentName,
        endpoint: settings.endpoint,
        apiKey: settings.apiKey)
    .Build();

string userInput;

Console.Write("You: ");
userInput = Console.ReadLine();
await foreach (var update in kernel.InvokePromptStreamingAsync(userInput))
{
    Console.Write(update);
}
