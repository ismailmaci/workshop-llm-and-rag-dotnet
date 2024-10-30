using Microsoft.SemanticKernel;
using Kernel = Microsoft.SemanticKernel.Kernel;

var (deploymentName, endpoint, apiKey) = LlmService.LlmService.LoadSettings();
PromptExecutionSettings settings = new()
        {
            ExtensionData = new Dictionary<string, object>()
            {
                { "Temperature", 2 },
                { "TopP", 0.5 },
                { "MaxTokens", 1024 },
                { "FormatSetting", "json_schema" }
            }
        };

Kernel kernel = Kernel.CreateBuilder()
    .AddAzureOpenAIChatCompletion(
        deploymentName: deploymentName,
        endpoint: endpoint,
        apiKey: apiKey)
    .Build();

Console.Write("You: ");
var userInput = Console.ReadLine() ?? string.Empty;
await foreach (var update in kernel.InvokePromptStreamingAsync(userInput, new KernelArguments(settings)))
{
    await Task.Delay(50);
    Console.Write(update);
}
