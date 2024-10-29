using Microsoft.SemanticKernel;
using Kernel = Microsoft.SemanticKernel.Kernel;

Kernel kernel = Kernel.CreateBuilder()
    .AddAzureOpenAIChatCompletion(
        deploymentName: "",
        endpoint: "",
        apiKey: "")
    .Build();

string userInput;

Console.Write("You: ");
userInput = Console.ReadLine();

KernelPlugin conversationSummaryPlugin = kernel.ImportPluginFromType<AssistantPlugin>();

await foreach (var update in kernel.InvokeStreamingAsync(conversationSummaryPlugin["GetPassword"], new() { ["input"] = userInput }))
{
    Console.Write(update);
}
