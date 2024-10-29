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

KernelPlugin conversationSummaryPlugin = kernel.ImportPluginFromType<ExercisePlugin>();

await foreach (var update in kernel.InvokeStreamingAsync(conversationSummaryPlugin["ListExercises"]))
{
    Console.Write(update);
}
