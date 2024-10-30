using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using Kernel = Microsoft.SemanticKernel.Kernel;
#pragma warning disable SKEXP0001

var settings = LlmService.LlmService.LoadSettings();
var builder = Kernel.CreateBuilder()
    .AddAzureOpenAIChatCompletion(
        deploymentName: settings.deploymentName,
        endpoint: settings.endpoint,
        apiKey: settings.apiKey);
builder.Plugins.AddFromType<CharacterCountPlugin>();
var kernel = builder.Build();

var chatCompletionService = kernel.GetRequiredService<IChatCompletionService>();
OpenAIPromptExecutionSettings openAIPromptExecutionSettings = new() 
{
    FunctionChoiceBehavior = FunctionChoiceBehavior.Auto()
};

Console.Write("You: ");
var userInput = Console.ReadLine() ?? "";

var result = await chatCompletionService.GetChatMessageContentAsync(
    prompt: userInput,
    executionSettings: openAIPromptExecutionSettings,
    kernel: kernel);

Console.Write(result);
