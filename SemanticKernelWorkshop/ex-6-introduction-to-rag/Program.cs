using Microsoft.SemanticKernel.Connectors.AzureOpenAI;
using Microsoft.SemanticKernel.Memory;

#pragma warning disable SKEXP0001
#pragma warning disable SKEXP0010
#pragma warning disable SKEXP0050

const string favoritePokemonCollection = "FavoritePokemonCollection";

var textEmbeddingService = new AzureOpenAITextEmbeddingGenerationService(
    deploymentName: "embedding",
    endpoint: "https://oai-aitoday-01.openai.azure.com",
    apiKey: "");
var memory = new SemanticTextMemory(new VolatileMemoryStore(), textEmbeddingService);

await memory.SaveInformationAsync(favoritePokemonCollection, "Pikachu, The Sheik", "pokemon1");
await memory.SaveInformationAsync(favoritePokemonCollection, "Charizard, The Ultimate Warrior", "pokemon2");
await memory.SaveInformationAsync(favoritePokemonCollection, "Blastoise, The Rock", "pokemon3");
await memory.SaveInformationAsync(favoritePokemonCollection, "Venusaur, Nature Boy", "pokemon4");
await memory.SaveInformationAsync(favoritePokemonCollection, "Snorlax, The Hulk", "pokemon5");
await memory.SaveInformationAsync(favoritePokemonCollection, "Espeon, The Undertaker", "pokemon6");

var question = @"What is the name of the Pokemon that is known as ""Nature Boy""?";

var response = memory.SearchAsync(favoritePokemonCollection, question, 1, 0.8);

await foreach (var item in response)
{
    Console.Write(item?.Metadata.Text);
}
