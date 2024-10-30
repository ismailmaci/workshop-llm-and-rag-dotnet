using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using Kernel = Microsoft.SemanticKernel.Kernel;
#pragma warning disable SKEXP0001

var (deploymentName, endpoint, apiKey) = LlmService.LlmService.LoadSettings();
var builder = Kernel.CreateBuilder()
    .AddAzureOpenAIChatCompletion(
        deploymentName: deploymentName,
        endpoint: endpoint,
        apiKey: apiKey);

var kernel = builder.Build();

Console.Write("You: ");
var userInput = Console.ReadLine() ?? "";


