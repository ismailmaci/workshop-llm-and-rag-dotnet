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
var userInput = Console.ReadLine() ?? "";

KernelPlugin conversationSummaryPlugin = kernel.ImportPluginFromType<AssistantPlugin>();

await foreach (var update in kernel.InvokeStreamingAsync(conversationSummaryPlugin["GetPassword"], new() { ["input"] = userInput }))
{
    Console.Write(update);
}
