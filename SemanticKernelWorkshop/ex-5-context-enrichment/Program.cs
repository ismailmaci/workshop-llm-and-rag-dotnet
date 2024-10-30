using Microsoft.SemanticKernel;
using Kernel = Microsoft.SemanticKernel.Kernel;

var (deploymentName, endpoint, apiKey) = LlmService.LlmService.LoadSettings();
Kernel kernel = Kernel.CreateBuilder()
    .AddAzureOpenAIChatCompletion(
        deploymentName: deploymentName,
        endpoint: endpoint,
        apiKey: apiKey)
    .Build();


var prompt = @"Suggest exercises for me."
    + "If none can be found, apologize to the user and give no other exercises."

    + "You can only choose exercised from the following list:"
    + "{{ExercisePlugin.ListExercises}}"
    + "Never provide an exercise that is not present in this list."

    + "Here are the recent exercises that I have performed:"
    + "{{ExercisePlugin.ListRecentExercises}}"
    
    + "Based on my recent activity, suggest 3 cardio and 3 resistance exercises if possible."
;

await foreach (var update in kernel.InvokePromptStreamingAsync(prompt))
{
    Console.Write(update);
}
