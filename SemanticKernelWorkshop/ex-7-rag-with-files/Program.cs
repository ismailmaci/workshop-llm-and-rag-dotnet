using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.AzureOpenAI;
using Microsoft.SemanticKernel.Memory;
using Microsoft.SemanticKernel.TextToImage;

#pragma warning disable SKEXP0001, SKEXP0010, SKEXP0050

const string pokedexEntryCollection = "PokedexEntryCollection";

var embeddingSettings = LlmService.LlmService.LoadEmbeddingSettings();
var textEmbeddingService = new AzureOpenAITextEmbeddingGenerationService(
    deploymentName: embeddingSettings.deploymentName,
    endpoint: embeddingSettings.endpoint,
    apiKey: embeddingSettings.apiKey);

var memory = new SemanticTextMemory(new VolatileMemoryStore(), textEmbeddingService);

var directory = Directory.GetCurrentDirectory();
var pokedex = File.ReadAllLines(Path.Combine(directory, "resources/pokedex.txt"));
var i = 0;

foreach (var entry in pokedex)
{
    await memory.SaveInformationAsync(pokedexEntryCollection, entry, i++.ToString());
}

string ask = "I love grass type pokemon, can you name 5 for me?";
Console.WriteLine("===========================\n" +
                    "Query: " + ask + "\n");

var memories = memory.SearchAsync(pokedexEntryCollection, ask, limit: 5, minRelevanceScore: 0.77);

i = 0;
await foreach (var m in memories)
{
    Console.WriteLine($"Result {++i}:");
    Console.WriteLine("  Title    : " + m.Metadata.Text);
    Console.WriteLine("  Relevance: " + m.Relevance);
    Console.WriteLine();
}

var settings = LlmService.LlmService.LoadSettings();
Kernel kernel = Kernel.CreateBuilder()
    .AddAzureOpenAIChatCompletion(
        deploymentName: settings.deploymentName,
        endpoint: settings.endpoint,
        apiKey: settings.apiKey)
    .AddOpenAITextToImage(
        //azure instance does not have a text-to-image service, resort to the openai service
        apiKey: "")
    .Build();

var dallE = kernel.GetRequiredService<ITextToImageService>();
var imageUrl = await dallE.GenerateImageAsync(memories.ToBlockingEnumerable().FirstOrDefault().Metadata.Text, 512, 512);
Console.WriteLine("Image URL: " + imageUrl);
