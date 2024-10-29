using Microsoft.SemanticKernel.Connectors.AzureOpenAI;
using Microsoft.SemanticKernel.Memory;

#pragma warning disable SKEXP0001, SKEXP0010, SKEXP0050

const string pokedexEntryCollection = "PokedexEntryCollection";

var textEmbeddingService = new AzureOpenAITextEmbeddingGenerationService(
    deploymentName: "embedding",
    endpoint: "https://oai-aitoday-01.openai.azure.com",
    apiKey: "");

var memory = new SemanticTextMemory(new VolatileMemoryStore(), textEmbeddingService);

var directory = Directory.GetCurrentDirectory();
var pokedex = File.ReadAllLines(Path.Combine(directory, "resources/pokedex.txt"));
var i = 0;

foreach (var entry in pokedex)
{
    await memory.SaveInformationAsync(pokedexEntryCollection, entry, i++.ToString());
}

string ask = "I love fire type pokemon, can you name 5 for me?";
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

