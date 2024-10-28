using Microsoft.SemanticKernel;
using Kernel = Microsoft.SemanticKernel.Kernel;

Kernel kernel = Kernel.CreateBuilder()
    .AddOpenAIChatCompletion(
        modelId: "gpt-4o-mini",
        apiKey: "")
    .Build();

string userInput;

Console.Write("You: ");
userInput = Console.ReadLine();
await foreach (var update in kernel.InvokePromptStreamingAsync(userInput))
{
    Console.Write(update);
}
