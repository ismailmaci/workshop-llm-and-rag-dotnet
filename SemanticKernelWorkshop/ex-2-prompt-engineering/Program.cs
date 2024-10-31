using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Experimental.Agents;
using Kernel = Microsoft.SemanticKernel.Kernel;

var (deploymentName, endpoint, apiKey) = LlmService.LlmService.LoadSettings();

Kernel kernel = Kernel.CreateBuilder()
    .AddAzureOpenAIChatCompletion(
        deploymentName: deploymentName,
        endpoint: endpoint,
        apiKey: apiKey)
    .Build();

// Zero-shot prompting. Try to translate something from English to French.
var zeroShotPrompt = "Translate the following English text to French: 'Hello, how are you?'";
var zeroShotReply = await kernel.InvokePromptAsync(zeroShotPrompt);
Console.WriteLine("Zero-shot Prompting: " + zeroShotReply);

// One-shot prompting. Try to translate something from English to French.
var oneShotPrompt = @"
Translate the following English text to French:
English: 'Good morning'
French: 'Bonjour'

English: 'Good night'
French: ";
var oneShotReply = await kernel.InvokePromptAsync(oneShotPrompt);
Console.WriteLine("One-shot Prompting: " + oneShotReply);

// Few-shot prompting
var fewShotPrompt = @"
Convert the given amount from Euros to US Dollars using the following examples:
€10 -> $11.00
€20 -> $22.00
Do not offer your thought process, just the result!

€50 ->
";
var fewShotReply = await kernel.InvokePromptAsync(fewShotPrompt);
Console.WriteLine("€50 -> " + fewShotReply.ToString().Trim());

// Chain-of-thought prompting
var chainOfThoughtPrompt = @"
Antwerp is 50 km north of Brussels. Ghent is 60 km west of Brussels.
Assuming there's a direct road from Antwerp to Ghent, how many kilometers will you travel in total?
Please explain your reasoning step by step.
";
var chainOfThoughtReply = await kernel.InvokePromptAsync(chainOfThoughtPrompt);
Console.WriteLine("Answer:\n" + chainOfThoughtReply);

// Using system prompts
var systemPrompt = @"
You are an expert travel assistant specialized in Belgian tourism and culinary delights.
Respond in a friendly, helpful, and informative manner.
Always end the conversation with a friendly goodbye message addressing them in regional slang.
Respond entirely in dutch.
";

ChatCompletionAgent chatAgent = new ChatCompletionAgent(kernel, systemPrompt);
var response = await chatAgent.InvokeAsync([new ChatMessageContent(AuthorRole.User, "Can you recommend some attractions in Geraardsbergen?")]);
foreach (var message in response)
{
    Console.WriteLine("Recommendations:\n" + message.ToString());
}
