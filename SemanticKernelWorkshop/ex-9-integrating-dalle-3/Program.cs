
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.TextToImage;
#pragma warning disable SKEXP0010
#pragma warning disable SKEXP0001

var (deploymentName, endpoint, apiKey) = LlmService.LlmService.LoadSettings();
var kernel = Kernel.CreateBuilder()
    .AddAzureOpenAIChatCompletion(
        deploymentName: deploymentName,
        endpoint: endpoint,
        apiKey: apiKey)
    .AddOpenAITextToImage(
        //azure instance does not have a text-to-image service, resort to the openai service
        apiKey: "")
    .Build();

var dallE = kernel.GetRequiredService<ITextToImageService>();
var imageUrl = await dallE.GenerateImageAsync("A corgi", 512, 512);
Console.WriteLine("Image URL: " + imageUrl);
