using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.AzureOpenAI;
using Microsoft.SemanticKernel.Memory;
using Microsoft.SemanticKernel.Plugins.Memory;

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

var questions = new[]
{
    @"What is the name of the Pokemon that is known as ""Nature Boy""?",
    "What is the name of the namename of Charizard?",
    "Who is the best psychic type?"
};

foreach (var q in questions)
{
    var response = memory.SearchAsync(favoritePokemonCollection, q).ToBlockingEnumerable().FirstOrDefault();
    Console.WriteLine("Q: " + q);
    Console.WriteLine("A: " + response?.Relevance.ToString() + "\t" + response?.Metadata.Text);
}

Kernel kernel = Kernel.CreateBuilder()
    .AddAzureOpenAIChatCompletion(
        deploymentName: "gpt-4o",
        endpoint: "https://oai-aitoday-01.openai.azure.com",
        apiKey: "")
    .Build();
    
kernel.ImportPluginFromObject(new TextMemoryPlugin(memory));

const string skPrompt = @"
ChatBot can have a conversation with you about any topic.
It can give explicit instructions or say 'I don't know' if it does not have an answer.

Information about me, from previous conversations:
- {{$fact1}} {{recall $fact1}}
- {{$fact2}} {{recall $fact2}}
- {{$fact3}} {{recall $fact3}}

Chat:
{{$history}}
User: {{$userInput}}
ChatBot: ";

var chatFunction = kernel.CreateFunctionFromPrompt(skPrompt, new AzureOpenAIPromptExecutionSettings { MaxTokens = 200, Temperature = 0.8 });

var arguments = new KernelArguments();

arguments["fact1"] = "Who is The Rock?";
arguments["fact2"] = "How many pokemon are named after WWE Wrestlers?";
arguments["fact3"] = "What pokemon is psychic?";

arguments[TextMemoryPlugin.CollectionParam] = favoritePokemonCollection;
arguments[TextMemoryPlugin.LimitParam] = "2";
arguments[TextMemoryPlugin.RelevanceParam] = "0.8";

var history = "";
arguments["history"] = history;
Func<string, Task> Chat = async (string input) => {
    // Save new message in the kernel arguments
    arguments["userInput"] = input;

    // Process the user message and get an answer
    var answer = await chatFunction.InvokeAsync(kernel, arguments);

    // Append the new interaction to the chat history
    var result = $"\nUser: {input}\nChatBot: {answer}\n";

    history += result;
    arguments["history"] = history;
    
    // Show the bot response
    Console.WriteLine(result);
};

await Chat("Hello, can you tell me what actor the nickname for Blastoise is named after?");
await Chat("Could you please tell me what pokemon is psychic from our previous conversation?");

