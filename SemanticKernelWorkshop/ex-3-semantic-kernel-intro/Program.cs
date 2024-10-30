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

KernelPlugin conversationSummaryPlugin = kernel.ImportPluginFromType<AssistantPlugin>();

await foreach (var update in kernel.InvokeStreamingAsync(conversationSummaryPlugin["GetPassword"], new() { ["input"] = userInput }))
{
    Console.Write(update);
}
