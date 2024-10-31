using Microsoft.SemanticKernel.Connectors.AzureOpenAI;
using Microsoft.SemanticKernel.Memory;

#pragma warning disable SKEXP0001, SKEXP0010, SKEXP0050

const string pokedexEntryCollection = "PokedexEntryCollection";
const string recipeEntryCollection = "RecipeEntryCollection";
const string dndEntryCollection = "DndEntryCollection";

var (deploymentNameEmbedding, endpointEmbedding, apiKeyEmbedding) = LlmService.LlmService.LoadEmbeddingSettings();
var textEmbeddingService = new AzureOpenAITextEmbeddingGenerationService(
    deploymentName: deploymentNameEmbedding,
    endpoint: endpointEmbedding,
    apiKey: apiKeyEmbedding);

var memory = new SemanticTextMemory(new VolatileMemoryStore(), textEmbeddingService);
var directory = Directory.GetCurrentDirectory();
var pokedex = File.ReadAllText(Path.Combine(directory, "resources/pokedex.txt"));
var recipes = File.ReadAllText(Path.Combine(directory, "resources/recipes.txt"));
var bestiary = File.ReadAllText(Path.Combine(directory, "resources/dnd_bestiary.txt"));

var i = 0;
var pokedexEntries = pokedex.Split("Name: ").Skip(1);
foreach (var entry in pokedexEntries)
{
    await memory.SaveInformationAsync(pokedexEntryCollection, entry, i++.ToString());
}

var recipeEntries = recipes.Split("(02) 8188 8722 | HelloFresh.com.au").Skip(1);
foreach (var entry in recipeEntries)
{
    await memory.SaveInformationAsync(recipeEntryCollection, entry, i++.ToString());
}

var dndEntries = bestiary.Split("====").Skip(1);
foreach (var entry in dndEntries)
{
    await memory.SaveInformationAsync(dndEntryCollection, entry, i++.ToString());
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
    Console.WriteLine();
}

string askRecipe = "Name me 3 recipes with salmon.";
Console.WriteLine("===========================\n" +
                    "Query: " + askRecipe + "\n");

var recipeMemories = memory.SearchAsync(recipeEntryCollection, askRecipe, limit: 3, minRelevanceScore: 0.10);
i = 0;
await foreach (var m in recipeMemories)
{
    Console.WriteLine($"Result {++i}:");
    Console.WriteLine("  Title    : " + m.Metadata.Text);
    Console.WriteLine();
}

string askDnd = "List 2 dragons.";
Console.WriteLine("===========================\n" +
                    "Query: " + askDnd + "\n");

var dndMemories = memory.SearchAsync(dndEntryCollection, askDnd, limit: 2, minRelevanceScore: 0.10);
i = 0;
await foreach (var m in dndMemories)
{
    Console.WriteLine($"Result {++i}:");
    Console.WriteLine("  Title    : " + m.Metadata.Text);
    Console.WriteLine();
}
