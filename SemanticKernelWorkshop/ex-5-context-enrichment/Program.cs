using Microsoft.SemanticKernel;
using Kernel = Microsoft.SemanticKernel.Kernel;

var (deploymentName, endpoint, apiKey) = LlmService.LlmService.LoadSettings();
Kernel kernel = Kernel.CreateBuilder()
    .AddAzureOpenAIChatCompletion(
        deploymentName: deploymentName,
        endpoint: endpoint,
        apiKey: apiKey)
    .Build();

kernel.ImportPluginFromType<ExercisePlugin>();

var prompt = @"Suggest exercises for me."

    + "Here is the list you can choose from:"
    + "{{ExercisePlugin.ListExercises}}"

    + "Here are the recent exercises that I have performed:"
    + "{{ExercisePlugin.ListRecentExercises}}"
    
    + "Based on my recent activity, suggest 3 cardio and 3 resistance exercises.";

await foreach (var update in kernel.InvokePromptStreamingAsync(prompt))
{
    Console.Write(update);
}
