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

KernelPlugin conversationSummaryPlugin = kernel.ImportPluginFromType<CharacterCountPlugin>();

await foreach (var update in kernel.InvokeStreamingAsync(conversationSummaryPlugin["GetRCharacters"], new() { ["input"] = userInput }))
{
    Console.Write(update);
}
