using Microsoft.SemanticKernel.Connectors.AzureOpenAI;
using Microsoft.SemanticKernel.Memory;
using Microsoft.Extensions.Logging;

#pragma warning disable SKEXP0001, SKEXP0010, SKEXP0050

const string pokedexEntryCollection = "PokedexEntryCollection";

var (deploymentNameEmbedding, endpointEmbedding, apiKeyEmbedding) = LlmService.LlmService.LoadEmbeddingSettings();
var textEmbeddingService = new AzureOpenAITextEmbeddingGenerationService(
    deploymentName: deploymentNameEmbedding,
    endpoint: endpointEmbedding,
    apiKey: apiKeyEmbedding,
    loggerFactory: LoggerFactory.Create(builder => builder.SetMinimumLevel(LogLevel.Trace)));

var memory = new SemanticTextMemory(new VolatileMemoryStore(), textEmbeddingService);
var directory = Directory.GetCurrentDirectory();
var pokedex = File.ReadAllText(Path.Combine(directory, "resources/pokedex.txt"));
var i = 0;
var pokedexEntries = pokedex.Split("Name: ").Skip(1);
foreach (var entry in pokedexEntries)
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

