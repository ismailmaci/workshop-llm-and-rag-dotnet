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

KernelPlugin conversationSummaryPlugin = kernel.ImportPluginFromType<CharacterCountPlugin>();

await foreach (var update in kernel.InvokeStreamingAsync(conversationSummaryPlugin["GetRCharacters"], new() { ["input"] = userInput }))
{
    Console.Write(update);
}
