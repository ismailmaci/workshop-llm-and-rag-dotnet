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
var zeroShotPrompt = "";
var zeroShotReply = await kernel.InvokePromptAsync(zeroShotPrompt);
Console.WriteLine("Zero-shot Prompting: " + zeroShotReply);

// One-shot prompting. Try to translate something from English to French.
var oneShotPrompt = @"";
var oneShotReply = await kernel.InvokePromptAsync(oneShotPrompt);
Console.WriteLine("One-shot Prompting: " + oneShotReply);

// Few-shot prompting. Try to convert Euros to US Dollars.
var fewShotPrompt = @"";
var fewShotReply = await kernel.InvokePromptAsync(fewShotPrompt);
Console.WriteLine(fewShotReply.ToString().Trim());

// Chain-of-thought prompting. Try to calculate the distance between Antwerp and Ghent.
var chainOfThoughtPrompt = @"";
var chainOfThoughtReply = await kernel.InvokePromptAsync(chainOfThoughtPrompt);
Console.WriteLine("Answer:\n" + chainOfThoughtReply);

// Using system prompts. Try to impersonate the LLM as a travel assistant.
var systemPrompt = @"";

ChatCompletionAgent chatAgent = new ChatCompletionAgent(kernel, systemPrompt);
var response = await chatAgent.InvokeAsync([new ChatMessageContent(AuthorRole.User, "Can you recommend some attractions in Geraardsbergen?")]);
foreach (var message in response)
{
    Console.WriteLine("Recommendations:\n" + message.ToString());
}
